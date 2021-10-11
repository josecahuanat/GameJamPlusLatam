using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static int playerHealth, enemies;


    public RectTransform canvas;
    public Image fadeImage;
    public Text hourText, minuteText, dotsText;
    public float textGameplayOffset;
    public float animationDuration;
    public Ease animationEase;
    public GameObject playButton, quitButton;


    [Header("LEVEL")]
    public Player player;
    public float playerHourScale, playerHourSpeed;
    public float enemyMaxSpeed, enemyMinuteSpeed;
    public Transform[] spawnPositions;
    public GameObject enemyPrefab;


    int hour, minute;
    RectTransform hourRT, minuteRT;
    Vector2 hourMenuPos, minuteMenuPos;
    Vector2 hourMenuSize, minuteMenuSize;
    List<Enemy> enemiesSpawned;


    bool _playing;
    bool playing
    {
        get{ return _playing; }
        set
        {
            if (playing == value)
                return;

            _playing = value;

            dotsText.DOKill();
            dotsText.text = string.Empty;

            if (playing)
            {
                fadeImage.DOFade(0f, animationDuration).SetEase(animationEase);
                CreateLevel();
            }
            else
            {
                fadeImage.DOFade(.85f, animationDuration).SetEase(animationEase);

                hourRT.DOAnchorPos(hourMenuPos, animationDuration).SetEase(animationEase);
                hourRT.DOSizeDelta(hourMenuSize, animationDuration).SetEase(animationEase);

                minuteRT.DOAnchorPos(minuteMenuPos, animationDuration).SetEase(animationEase);
                minuteRT.DOSizeDelta(minuteMenuSize, animationDuration).SetEase(animationEase);

                dotsText.DOText(":", animationDuration, false, ScrambleMode.None).SetEase(animationEase).SetLoops(-1, LoopType.Yoyo);

                foreach(var enemySpawned in enemiesSpawned)
                {
                    if (enemySpawned != null)
                    {
                        enemySpawned.enabled = false;
                        enemySpawned.transform.DOScale(Vector3.zero, animationDuration)
                            .SetEase(animationEase).OnComplete(() => Destroy(enemySpawned));
                    }
                }

                player.enabled = false;
                player.transform.DOKill();
                player.transform.DOScale(Vector3.zero, animationDuration).SetEase(animationEase).OnComplete(() =>
                {
                    playButton.SetActive(true);
                    quitButton.SetActive(true);
                });

            }
        }
    }


    void Start()
    {
        hourRT = hourText.GetComponent<RectTransform>();
        hourMenuPos = hourRT.anchoredPosition;
        hourMenuSize = hourRT.sizeDelta;

        minuteRT = minuteText.GetComponent<RectTransform>();
        minuteMenuPos = minuteRT.anchoredPosition;
        minuteMenuSize = minuteRT.sizeDelta;

        dotsText.text = string.Empty;
        dotsText.DOText(":", animationDuration, false, ScrambleMode.None).SetEase(animationEase).SetLoops(-1, LoopType.Yoyo);

        player.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (playing)
        {
            if (player.enabled)
            {
                minuteText.text = enemies.ToString("00");
                hourText.text = playerHealth.ToString("00");

                if (enemies == 0 || playerHealth == 0)
                {
                    playing = false;
                }
            }
        }
        else
        {
            SetTime();
        }
    }

    public void Play()
    {
        playing = true;
    }

    public void GoToMainMenu()
    {
        playing = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    void SetTime()
    {
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;

        // Debug.Log($"{hour}, {minute}");

        hourText.text = hour.ToString("00");
        minuteText.text = minute.ToString("00");
    }

    void CreateLevel()
    {
        minute = 2;
        hour = 1;

        playButton.SetActive(false);
        quitButton.SetActive(false);

        var hourGameplayPos = new Vector2((canvas.sizeDelta.x / -2f) + textGameplayOffset, (canvas.sizeDelta.y / 2f));
        hourRT.DOAnchorPos(hourGameplayPos, animationDuration).SetEase(animationEase);
        hourRT.DOSizeDelta(new Vector2(hourMenuSize.x / 2f, hourMenuSize.y / 2f), animationDuration).SetEase(animationEase);

        float delay = animationDuration;
        const float shortAnimDuration = .2f;

        playerHealth = 0;
        enemies = 0;

        player.speed = player.defaultSpeed;

        for(int i=0 ; i<=hour ; i++)
        {
            int currentIndex = i;
            playerHealth = currentIndex;
            Vector3 scale = Vector3.one + Vector3.one * (i * playerHourScale);
            player.transform.DOScale(scale, shortAnimDuration).SetEase(animationEase).SetDelay(delay).OnComplete(
                () => hourText.text = $"{currentIndex.ToString("00")}/{hour.ToString("00")}");
            player.speed -= playerHourSpeed;
            delay += shortAnimDuration;
        }

        delay += .5f;

        var minuteGameplayPos = new Vector2((canvas.sizeDelta.x / 2f) - textGameplayOffset, (canvas.sizeDelta.y / 2f));
        minuteRT.DOAnchorPos(minuteGameplayPos, animationDuration).SetEase(animationEase).SetDelay(delay);
        minuteRT.DOSizeDelta(new Vector2(minuteMenuSize.x / 2f, minuteMenuSize.y / 2f), animationDuration).SetEase(animationEase).SetDelay(delay);

        delay += animationDuration;

        var enemyPositions = spawnPositions.ToList();
        enemiesSpawned = new List<Enemy>();

        for(int i=0 ; i<=minute-2 ; i+=2)
        {
            int spawnIndex = Random.Range(0, enemyPositions.Count);
            float randomRotation = Random.Range(0f, 360f);
            var enemy = GameObject.Instantiate(enemyPrefab, enemyPositions[spawnIndex].position, Quaternion.Euler(0f, 0f, randomRotation)).transform;
            enemy.localScale = Vector3.zero;
            int currentIndex = i;
            enemy.DOScale(Vector3.one, shortAnimDuration).SetEase(animationEase).SetDelay(delay).OnComplete(() =>
            {
                enemies++;
                minuteText.text = $"{enemies.ToString("00")}/{((minute-2) - currentIndex).ToString("00")}";
            });
            enemiesSpawned.Add(enemy.GetComponent<Enemy>());
            enemyPositions.RemoveAt(spawnIndex);
            delay += shortAnimDuration;
        }

        float enemySpeed = enemyMaxSpeed - (enemyMinuteSpeed * enemiesSpawned.Count);

        delay += .5f;
        DOVirtual.DelayedCall(delay, () =>
        {
           player.enabled = true;

           foreach(var enemySpawned in enemiesSpawned)
           {
               enemySpawned.speed = enemySpeed;
               enemySpawned.enabled = true;

           }
           minuteText.text = enemies.ToString("00");
           hourText.text = playerHealth.ToString("00");
        });
    }
}
