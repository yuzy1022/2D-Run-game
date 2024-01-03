using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour {
    public static GameManager instance; // 싱글톤을 할당할 전역 변수

    public bool isGameover = false; // 게임 오버 상태
    public static int heartScore=2;
    public Text scoreText; // 점수를 출력할 UI 텍스트
    public Text heartText;
    public GameObject jumpUpImage;
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트

    public static int score = 0; // 게임 점수

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake() {  //start와 비슷, start는 스크립트가 활성화 될때 한번만 실행되지만 awake는 활성화 되든 안되든 실행된다
        // 싱글톤 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;  //this는 해당 스크립트를 뜻한다.  싱글톤으로 해당 스크립트를 인스턴스화 했다.
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우

            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다는 의미.
            // 싱글톤 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);  //gameobject는 유니티 내의 오브젝트를 뜻한다.
        }
    }

    void Update() {
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if(isGameover && Input.GetMouseButton(0)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //현재 활성화 되어있는 씬을 재시작(Main Scene)
            PlatformSpawner.createPlatform=true;
            heartScore = 2;
            heartT();
            PlatformSpawner.stepCount=0;
            PlatformSpawner.createPlatform = true;
            PlayerController.jumpForce = 700;
            PlayerController.jumpUpTimer = 0;
            score = 0;

        }
        if(PlayerController.jumpForce==1000) jumpUpImage.SetActive(true);
        else if(PlayerController.jumpForce==700) jumpUpImage.SetActive(false);
    }

    
    public void AddScore(int newScore) {
        // 점수를 증가시키는 메서드
        if(!isGameover){  //게임 오버가 아니라면
            score += newScore; //점수를 증가
            scoreText.text = "Score : " + score;  //연산자 오버로딩으로 문자열과 숫자를 더할 수 있다.
        }
    }

    public void HeartScore(int h){
        if(heartScore < 3 && h > 0){heartScore+=1; heartT();}
        else if(heartScore > 0 && h < 0){heartScore-=1; heartT();}
        else if(heartScore==0 && h<0){
            PlayerController.animator.SetTrigger("Die");
            OnPlayerDead();
            }

    }

    public void heartT(){
        switch(heartScore){
            case 0: heartText.text=""; break;
            case 1: heartText.text="♥"; break;
            case 2: heartText.text="♥♥"; break;
            case 3: heartText.text="♥♥♥"; break;
        }
    }

    
    public void OnPlayerDead() {
        // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
        isGameover = true;
        gameoverUI.SetActive(true);
    }
}