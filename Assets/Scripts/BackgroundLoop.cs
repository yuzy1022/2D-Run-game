﻿using UnityEngine;

// 왼쪽 끝으로 이동한 배경을 오른쪽 끝으로 재배치하는 스크립트
public class BackgroundLoop : MonoBehaviour {
    private float width; // 배경의 가로 길이

    private void Awake() {
        // 가로 길이를 측정하는 처리
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x; //박스콜라이더 size.x값을 가로 길이로 사용
    }

    private void Update() {
        // 현재 위치가 원점에서 왼쪽으로 width 이상 이동했을때 위치를 리셋
        if(transform.position.x <= -width){
            Reposition();
        }
    }

    
    private void Reposition() {
        // 위치를 리셋하는 메서드
        Vector2 offset = new Vector2(width*2f, 0); //현재 위치에서 오른쪽 가로 길이 *2만큼 이동
        transform.position = (Vector2)transform.position + offset;  //벡터3을 벡터2로 형변환ㅋ
    }
}