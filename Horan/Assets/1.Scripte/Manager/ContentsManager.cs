using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModes { StoryMode, DefanseMode, TutorialMode }
public class ContentsManager 
{
    /*  매니저의 기능
     * 각 모드마다 인게임 내용에 따라 변동되어 저장될 데이터들을 수집하여 게임이 종료될때 세이브파일의 내용과 덪씌워주는 역할
     * + 각 게임모드의 시작 종료시 처리되어야하는 동작들을 메서드로 구현
     * 
     * 스토리모드 : 게임 시작, 게임 종료 , 웨이브 시작 , 웨이브 종료 ,에 해당하는 함수를 ContentsManager에 구현(호출은 각 Scene에서)
     * 
     * 예시 동작 흐름)
     * 유저가 스토리 모드를 선택 -> 스테이지를 클릭 -> 씬 이동 -> 각 씬스크립트에서 게임시작 함수 호출 -> 스테이지 진행
     * 결국 각 씬의 시작과 다음씬으로 넘어가는 부분은 씬에 존재하는  씬스크립트에서 담당
     * 하지만 씬이 변경되더라도 인게임 정보를 가지고 넘어갈수 있도록 컨텐츠 매니저에서 데이터 관리
     * 
     * 이제 GameTest1씬에서 씬스크립트 만들고 거기서 컨텐츠매니저의 GameStart()를 호출하게 하는 방식으로 처리 
     * 
     * 아니 몬스터 들이 스폰되면 초기화시점에 OnStageClear에 자신이 죽었을때 점수를 
     * 
     * 정해진 수의 몬스터를 다 잡는다? 몬스터의 수는 어떻게 가져오지?
     * 몬스터가 생성될때 컨텐츠매니저에게 자신이 생성되있음을 알리자 
     * 그럼 curWaveMonsterCounts값을 증가시키고 
     * 죽을때 마다 해당 값을 하나씩 내린다
     * 
     * 값이 변할때 마다 해당 값이 0이되었는지 확인하고 0이 되었을때 게임이 종료된다. 
     * 
     * 인터페이스로 넘긴다?
     * 
     */
    int curStageindex = 0;
    int waveMonsterCounts;
    public int WaveMonsterCounts 
    { 
        get { return waveMonsterCounts; } 
        set { waveMonsterCounts = value; if (waveMonsterCounts == 0) if(OnWaveClear!=null) OnWaveClear.Invoke(); } 
    }
    

    //씬스크립트에서 해당 델리게이트 바인드해서 클리어시 맵이동되게 만들기
    public Action OnWaveClear; 


    public void StageStart(GameModes gameMode,int stage=1) //이거는 로비에서 선택용
    {
        switch (gameMode)
        {
            case GameModes.StoryMode:
                curStageindex = stage;
                break;
            case GameModes.DefanseMode:
                break;
            case GameModes.TutorialMode:
                break;
        }
    }
    public void StageEnd()
    {

        //Application.Quit();   
    }
    public void NextWave() //스테이지에서 다음 스테이지로 넘어갈때 호출
    {
        
    }

}

