using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    
    private List<Func<IEnumerator>> defaultPattern = new ();
    private List<Func<IEnumerator>> closeRange = new ();
    private List<Func<IEnumerator>> mediumRange = new ();
    private List<Func<IEnumerator>> longRange = new ();

    private Distance targetDistance = Distance.Default;

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

    protected void AddPattern(Distance distance, Func<IEnumerator> pattern)
    {
        switch (distance)
        {
            case Distance.CloseRange:
                closeRange.Add(pattern);
                break;
            case Distance.MediumRange:
                mediumRange.Add(pattern);
                break;
            case Distance.LongRange:
                longRange.Add(pattern);
                break;
            default:
                defaultPattern.Add(pattern);
                break;
        }
    }

    protected Func<IEnumerator> GetPattern()
    {
        List<Func<IEnumerator>> list = GetPatternList(targetDistance);
        if (list.Count == 0)
            return null;
        for (int i = 0; i < list.Count; i++)
        {
            int ran = Random.Range(0, list.Count);
            Func<IEnumerator> ranPattern = list[ran];
            list[ran] = list[i];
            list[i] = ranPattern;
            return ranPattern;
        }
        return list[0];
    }

    private List<Func<IEnumerator>> GetPatternList(Distance distance)
    {
        switch (distance)
        {
            case Distance.CloseRange:
                return closeRange;
            case Distance.MediumRange:
                return mediumRange;
            case Distance.LongRange:
                return longRange;
            default:
                return defaultPattern;
        }
    }
    
    protected void ClearPatternList(Distance distance)
    {
        GetPatternList(distance).Clear();
        targetDistance = Distance.Default;
    }
}   

