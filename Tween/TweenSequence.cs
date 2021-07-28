using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TweenSequence : Tween
{
    protected List<TweenData> _tweenDatas;
    [SerializeField] private StartType startType = StartType.byLocalCall;
    [SerializeField] private bool looping;
    public static Action OnSequencesAvake;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        if (startType == StartType.onAvake)
        {
            Init();
        }
        if(startType == StartType.byStaticCall)
        {
            OnSequencesAvake += Init;
        }
        OnTweenFinished = StartNextTween;
    }

    protected virtual void OnDestroy()
    {
            OnSequencesAvake -= Init;

    }

    public override void Init()
    {
        print("INIT!!!"+gameObject);
        if (startType == StartType.byStaticCall && !looping)
        {
            OnSequencesAvake -= Init;
        }

        if (_tweenDatas.Count > 0)
        {
            StartTween(_tweenDatas[0]);
        } 
    }

    protected virtual void StartNextTween(TweenData tweenData)
    {
        int index = _tweenDatas.FindIndex(x=> x == tweenData);
        if(index < _tweenDatas.Count - 1)
        {
            StartTweenAtIndex(index + 1);
        }
        else if (looping)
        {
            StartTweenAtIndex(0);
        }        
    }

    private void StartTweenAtIndex(int index)
    {
        StartTween(_tweenDatas[index]);
    }
}

[System.Serializable]
public enum StartType
{
    onAvake,byStaticCall,byLocalCall
}