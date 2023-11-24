using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPattern : MonoBehaviour
{
    protected enum Distance
    {
        Default,
        CloseRange,
        MediumRange,
        LongRange
    }
    
    protected enum PatternState
    {
       SUCCESS,
       RUNNING,
       FAILURE
    }

    private Dictionary<Distance, List<IEnumerator>> patternDict = new Dictionary<Distance, List<IEnumerator>>();

    private Distance targetDistance = Distance.Default;
    protected PatternState state;

    protected virtual Distance SetDistance(Vector3 targetPosition)
    {
        float distance = Mathf.Abs(targetPosition.x - transform.position.x);
        if (distance < 2f)
        {
            targetDistance = Distance.CloseRange;
        }
        else
        {
            targetDistance = distance < 6f ? Distance.MediumRange : Distance.LongRange;
        }
        return targetDistance;
    }

    protected void SetPattern(Distance distance, List<IEnumerator> pattern)
    {
        if (pattern != null)
            patternDict.Add(distance, pattern);
    }

    protected IEnumerator GetPattern()
    {
        if (!patternDict.ContainsKey(targetDistance))
            return null;
    
        List<IEnumerator> pattern = patternDict[targetDistance];
        int ran = Random.Range(0, pattern.Count);
        
        ShufflePattern(pattern);
        return pattern[ran];
    }
    
    protected void ClearPattern()
    {
        patternDict.Clear();
        targetDistance = Distance.Default;
    }

    private void ShufflePattern(List<IEnumerator> pattern)
    {
        for (int i = 0; i < pattern.Count; i++)
        {
            int ran = Random.Range(0, pattern.Count);
            IEnumerator temp = pattern[i];
            pattern[i] = pattern[ran];
            pattern[ran] = temp;
        }
    }
}   

