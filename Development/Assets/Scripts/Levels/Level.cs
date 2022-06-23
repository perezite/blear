using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class to control the progression logic within the scene
public class Level : MonoBehaviour
{
    // the ball
    private Ball ball;

    // the paddle
    private Paddle paddle;

    // the GUI
    private GUI gui;

    // the lose screen
    private LoseScreen loseScreen;

    // the win screen
    private WinScreen winScreen;

    // sound script
    private LevelSound levelSound;

    // ball prototype
    private GameObject ballPrototype;

    // lives of the player
    private int playerLives = 2;

    // score of the player
    private int playerScore = 100;

    // the current state
    private State currentState;

    // is a new state queued
    private bool stateQueued = false;

    // play winning sound
    private bool playWinningSound = true;

    // play losing sound
    private bool playLosingSound = true;

    // state enumeration
    public enum State
    {
        Aiming, Running, Dying, Winning, Losing, Unsticking
    }

    // set play winning sound
    public void SetPlayWinningSound(bool value)
    {
        playWinningSound = value;
    }

    // set play losing sound
    public void SetPlayLosingSound(bool value)
    {
        playLosingSound = value;
    }

    // queue state
    public void QueueState(State state)
    {
        currentState = state;
        stateQueued = true;
    }

    private static List<Brick> GetBricks()
    {
        var brickObjects = GameObject.FindGameObjectsWithTag("Brick").ToList();
        var bricks = brickObjects.Select(x => x.GetComponent<Brick>()).Where(x => x != null).ToList();
        return bricks;
    }

    private static List<Ball> GetActiveBalls()
    {
        var balls = GameObject.FindGameObjectsWithTag("Ball").Select(x => x.GetComponent<Ball>()).ToList();
        balls = balls.Where(x => x != null && x.GetCurrentState() != Ball.State.Dying).ToList();
        return balls;
    }

    private void Awake()
    {
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        paddle = GameObject.FindGameObjectWithTag("Paddle").GetComponent<Paddle>();
        gui = GameObject.FindGameObjectWithTag("GUI").GetComponent<GUI>();
        winScreen = GameObject.FindGameObjectWithTag("WinScreen").Try(x => x.GetComponent<WinScreen>());
        loseScreen = GameObject.FindGameObjectWithTag("LoseScreen").Try(x => x.GetComponent<LoseScreen>());
    }

    private IEnumerator Start()
    {
        // show an ad if the last negotiation was successful
        // this may be the case when the player previously clicked the 'home' button in the win screen
        if (AdManager.GetInstance().GetLastNegotiationResult() == true)
        {
            // yield return StartCoroutine(AdManager.GetInstance().ShowAd(AdManager.VideoZone));
			yield return null;
        }

        // prepare components
        levelSound = GetComponent<LevelSound>();
        gui.SetScore(playerScore);
        levelSound.Try(x => x.PlayStartSound());

        // create the ball prototype from the given ball in editor
        ballPrototype = (GameObject)Instantiate(ball.gameObject, ball.gameObject.transform.position, ball.gameObject.transform.rotation);
        ballPrototype.transform.parent = ball.gameObject.transform.parent;
        ballPrototype.SetActive(false);
        Destroy(ball.gameObject);

        // register brick events
        List<Brick> bricks = GetBricks();
        bricks.ForEach(x => x.DestructionPending += OnBrickDestroyed);

        // start state machine
        currentState = State.Aiming;
        StartCoroutine(StateMachine());
        yield return null;
    }

    // state machine
    private IEnumerator StateMachine()
    {
        while (true)
        {
            stateQueued = false;
            yield return StartCoroutine(currentState.ToString());
        }
    }

    private IEnumerator Aiming()
    {
        // prepare actors
        Ball ball = CloneBallFromPrototype();
        ball.QueueState(Ball.State.Aiming);
        paddle.QueueState(Paddle.State.Aiming);

        // perform aiming
        while (!stateQueued)
        {
            if (InputHelper.GetTapDown())
            {
                currentState = State.Running;
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Running()
    {
        // prepare actors
        var balls = GetActiveBalls().ToList();
        balls.ForEach(x => x.QueueState(Ball.State.Flying));
        paddle.QueueState(Paddle.State.Running);

        // wait for something to happen
        while (true)
        {
            // check if all bricks are destroyed
            if (AllBricksAreDestroyed())
            {
                currentState = State.Winning;
                yield break;
            }

            // check for ground hit
            var activeBalls = GetActiveBalls();
            var ballsTouchingGround = activeBalls.Where(x => BallTouchesGround(x)).ToList();
            var allBallsHaveHitGround = activeBalls.Count == ballsTouchingGround.Count;
            ballsTouchingGround.ForEach(x => x.QueueState(Ball.State.Dying));
            if (allBallsHaveHitGround)
            {
                currentState = State.Dying;
                yield break;
            }

            // external state change
            if (stateQueued)
            {
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Dying()
    {
        paddle.QueueState(Paddle.State.Paralysed);

        // wait a little
        yield return new WaitForSeconds(2.5f);

        // remove life
        playerLives = playerLives - 1;
        gui.RemoveHeart();

        // switch state
        if (playerLives < 0)
        {
            currentState = State.Losing;
        }
        else
        {
            currentState = State.Aiming;
        }
    }

    private IEnumerator Winning()
    {
        yield return new WaitForSeconds(0.1f);

        if (playWinningSound)
        {
            yield return levelSound.TryStartCoroutine(this, x => x.PlayWinSound());
        }
         
        GetActiveBalls().ForEach(x => x.QueueState(Ball.State.Paralysed));
        paddle.QueueState(Paddle.State.Paralysed);

        winScreen.gameObject.SetActive(true);
        winScreen.Activate();
        while (true)
        {
            if (winScreen.WasHomeButtonClicked())
            {
                GameManager.GetInstance().GoToMainMenu();
            }

            if (winScreen.WasProceedButtonClicked())
            {
                MusicManager.GetInstance().Next();
                yield return StartCoroutine(GameManager.GetInstance().GoToLevelWithLoadingIndicator(Application.loadedLevel + 1));
            }

            if (winScreen.WasPlayAgainButtonClicked())
            {
                MusicManager.GetInstance().Reset();
                yield return StartCoroutine(GameManager.GetInstance().GoToLevelWithLoadingIndicator(Application.loadedLevel));
            }

            yield return null;
        }
    }

    private IEnumerator Losing()
    {
        GameObject.FindGameObjectsWithTag("Ball").Select(x => x.GetComponent<Ball>()).ToList().ForEach(x => x.QueueState(Ball.State.Dying));
        yield return new WaitForSeconds(0.1f);

        if (playLosingSound)
        {
            yield return levelSound.TryStartCoroutine(this, x => x.PlayLoseSound());
        }

        paddle.QueueState(Paddle.State.Paralysed);

        if (loseScreen)
        {
            loseScreen.gameObject.SetActive(true);
            loseScreen.Activate();
            while (true)
            {
                if (loseScreen.IsReclaimLifeButtonClicked())
                {
                    playerLives += 1;
                    gui.AddHeart();
                    QueueState(State.Aiming);
                    loseScreen.Reset();
                    loseScreen.gameObject.SetActive(false);
                    break;
                }

                if (loseScreen.IsGameOverButtonClicked())
                {
                    MusicManager.GetInstance().Reset();
                    yield return StartCoroutine(GameManager.GetInstance().GoToLevelWithLoadingIndicator(Application.loadedLevel));
                    break;
                }

                yield return null;
            }
        }
    }

    private Ball CloneBallFromPrototype()
    {
        var ball = Instantiate(ballPrototype);
        ball.transform.parent = ballPrototype.transform.parent;
        ball.SetActive(true);
        return ball.GetComponent<Ball>();
    }

    private bool AllBricksAreDestroyed()
    {
        var intactBricks = GetBricks().Count(x => x.GetComponent<Brick>().IsDestructionPending() == false);
        return intactBricks == 0;
    }

    private bool BallTouchesGround(Ball ball)
    {
        var ballCollider = ball.gameObject.GetComponent<Collider2D>();
        var groundCollider = WorldManager.GetInstance().Ground.GetComponent<Collider2D>();
        return ballCollider.IsTouching(groundCollider);
    }

    private void OnBrickDestroyed(UnityEngine.Object sender)
    {
        playerScore += 100;
        gui.SetScore(playerScore);
    }
}