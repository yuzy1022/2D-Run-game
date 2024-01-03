using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public GameObject parent;
    public int count = 3; // 생성할 발판의 개수
    public static bool createPlatform = true;
    public static int stepCount = 0;
    public GameObject[] items = new GameObject[3];

    public float timeBetSpawnMin = 0f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 0f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 0.5f; // 배치할 위치의 최대 y값
    private float xPos = 17f; // 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -20); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점


    void Start() {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        platforms = new GameObject[count];
        for (int i = 0; i<count; i++){
            //platformPrefab을 원본으로 새 발판을 poolPosition위치에 복제 생성
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
            platforms[i].transform.parent = parent.transform;
        }
        /*lastSpawnTime = 0f; //마지막 배치 시점 초기화
        timeBetSpawn = 0f; //다음번 배치 까지의 시간 간격을 0으로 초기화*/
    }

    void Update() {
        // 순서를 돌아가며 주기적으로 발판을 배치
        if(GameManager.instance.isGameover) return;
        //if(Time.time >= lastSpawnTime + timeBetSpawn)  //마지막 배치 시점에서 timeBetSpawn 이상 시간이 흘렀다면
        if(createPlatform)
        {
           createPlatform=false;
            /*lastSpawnTime = Time.time; //기록된 마지막 배치 시점을 현재 시점으로 갱신
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);*/ //다음 배치까지의 간격을 timeBetSpawnMin, timeBetSpawnMax사이에서 랜덤 설정
            float yPos = Random.Range(yMin, yMax); //배치할 위치의 높이를 yMin과 yMax사이에서 랜덤 설정
            platforms[currentIndex].SetActive(false);  //현재 순번의 발판 게임 오브젝트를 비활성화 하고 즉시 다시 활성화. 이때 발판의 platform컴포넌트의 OnEable메소드가 실행됨
            platforms[currentIndex].SetActive(true);  
            int s;
            if(currentIndex==0) s=2;
            else s=currentIndex-1;
            platforms[currentIndex].transform.position = new Vector2(platforms[s].transform.position.x+xPos, yPos);  //현재순번의 발판을 화면 오른쪽에 재배치
            currentIndex++;
            if(currentIndex >= count){
                currentIndex = 0;
            }
            if(GameManager.score>=30) {xPos = 19; }
            else if(GameManager.score>=45) yMax=1;
            else if(GameManager.score>=60) xPos = 21;
            else if(GameManager.score>=100) xPos=22; yMax=2;
            
        }

        if(stepCount==10){
            PlatformSpawner.stepCount = 0;
            int random = Random.Range(0, 3);
            GameObject item = Instantiate(items[random], poolPosition, Quaternion.identity);
            item.transform.position = new Vector2(platforms[(currentIndex+1)%3].transform.position.x+10, platforms[(currentIndex+1)%3].transform.position.y+2);
            item.transform.parent = platforms[(currentIndex+1)%3].transform;
            Destroy(item, 5f);
        }
    }
}