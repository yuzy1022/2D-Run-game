using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public static float jumpForce = 700f; // 점프 힘
    public static float jumpUpTimer=0;
   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   public static Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() {
       // 초기화
       playerRigidbody = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       playerAudio = GetComponent<AudioSource>();
   }

   private void Update() {
       // 사용자 입력을 감지하고 점프하는 처리
       if(isDead){
           return;  //사망시 처리를 더이상 진행하지 않고 종료
       }

       if(Input.GetMouseButtonDown(0) && jumpCount < 2){ //마우스좌클릭을 했으며 최대 점프횟수(2)에 도달하지 않았으면
           jumpCount++; //점프횟수증가
           playerRigidbody.velocity = Vector2.zero;  //점프 직전 속도를 순간적으로 0으로 변경
           playerRigidbody.AddForce(new Vector2(0, jumpForce));  //위로 점프
           playerAudio.Play();  //오디오소스 재생
       }

       else if(Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0){ //좌클릭에서 손을 떼는 순간 && 속도의 y값이 양수라면(위로 상승중일때)
           playerRigidbody.velocity = playerRigidbody.velocity*0.5f;  //현재속도를 절반으로 변경
       }
       animator.SetBool("Grounded", isGrounded);  //애니메이터의 Grounded파라미터를 isGrounded로 갱신

       if(jumpUpTimer>0) jumpUpTimer -= Time.deltaTime;
       else if(jumpUpTimer<=0) jumpForce=700;
   }

   private void Die() {
       // 사망 처리
       animator.SetTrigger("Die");  //애니메이션의 Die파라미터를 실행
       playerAudio.clip = deathClip;  //deathClip 오디오 클립으로 변경
       playerAudio.Play();  //사망효과음 재생
       playerRigidbody.velocity = Vector2.zero;  //속도를 0으로 변경
       isDead = true;  //사망상태를 true로 변경
       GameManager.instance.OnPlayerDead(); //싱글톤으로 인스턴스화 한 게임매니저의 게임오버 함수 실행
   }

   private void OnTriggerEnter2D(Collider2D other) {
       // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
       if(other.tag == "Dead" && !isDead){  //충돌한 오브젝트의 태그가 Dead이며 아직 사망하지 않았다면 Die()실행
           GameManager.instance.HeartScore(-1);
           playerAudio.PlayOneShot(deathClip);
       }
       else if(other.tag == "Die" && !isDead) Die();
   }

   private void OnCollisionEnter2D(Collision2D collision) {
       // 바닥에 닿았음을 감지하는 처리
       if(collision.contacts[0].normal.y > 0.7f){  //어떤 콜라이더와 닿았으며 충돌 표면이 위쪽을 보고 있으면
           isGrounded = true;
           jumpCount = 0;
       }
   }

   private void OnCollisionExit2D(Collision2D collision) {
       // 바닥에서 벗어났음을 감지하는 처리
       isGrounded = false;
   }

   public static void jumpUp(){
        jumpForce=1000;
        jumpUpTimer=10;
   }
   
}