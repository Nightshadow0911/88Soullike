using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PattonTest : MonoBehaviour
{
    public List<BossPattern> patternList;

    private void Start()
    {
        StartBossBattle();
    }

    public void StartBossBattle()
    {
        StartCoroutine(ExecuteRandomPattern());
    }

    private IEnumerator ExecuteRandomPattern()
    {
        List<BossPattern> availablePatterns = patternList.FindAll(p => p.currentRepeatCount < p.maxRepeatCount);

        while (availablePatterns.Count > 0)
        {
            BossPattern selectedPattern = ChooseRandomPattern(availablePatterns);
            ExecutePattern(selectedPattern);

            selectedPattern.currentRepeatCount++;
            foreach (BossPattern pattern in patternList)
            {
                if (pattern != selectedPattern)
                {
                    pattern.currentRepeatCount = 0;
                }
            }

            yield return new WaitForSeconds(1.0f); // 패턴 간 딜레이
        }

        // 모든 패턴 실행 후 원하는 동작을 수행
    }

    private BossPattern ChooseRandomPattern(List<BossPattern> availablePatterns)
    {
        float totalProbability = 0f;
        foreach (var pattern in availablePatterns)
        {
            totalProbability += pattern.probability;
        }

        float randomValue = Random.Range(0f, totalProbability);

        foreach (var pattern in availablePatterns)
        {
            if (randomValue < pattern.probability)
            {
                return pattern;
            }
            randomValue -= pattern.probability;
        }

        return availablePatterns[0];
    }

    private void ExecutePattern(BossPattern pattern)
    {
        Debug.Log("Executing " + pattern.patternName);
        // 패턴 실행 로직을 여기에 추가
    }
    
    [System.Serializable]
    public class BossPattern
    {
        public string patternName;
        public float probability;// 패턴의 확률 
        public int maxRepeatCount; // 패턴 최대 반복 횟수
        public int currentRepeatCount; // 현재 반복 횟수
    }
}
