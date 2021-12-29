using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HahaCakesGoBrrr : MonoBehaviour
{
    [SerializeField, Tooltip("the rectangle in which the cakes will go brrr")]
    private Rect m_rect;

    [SerializeField, Tooltip("the gameOver tmp element")]
    private TextMeshProUGUI m_text;
    
    [SerializeField, Tooltip("the cake prefab")]
    private GameObject m_cakePrefab;
    
    [SerializeField, Tooltip("the time between cakes")]
    private float m_timeBetweenCakes = .1f;


    private void Start()
    {
        FunctionToMakeCakesGoBrr(PlayerPrefs.GetInt("LastScore"));
    }
    // Update is called once per frame
    public void FunctionToMakeCakesGoBrr(int p_score)
    {
        String highScoreText = PlayerPrefs.GetInt("NewHighScore") == 0 ? $" High Score : {PlayerPrefs.GetInt("HighScore")}" : "New High Score !";
        
        m_text.text = $"Game Over\nScore {p_score}\n{highScoreText}";
        StartCoroutine(MakeCakesGoBr(p_score));
    }

    IEnumerator MakeCakesGoBr(int pScore)
    {
        int cakesBrrrd = 0;
        while (cakesBrrrd< pScore)
        {
            yield return new WaitForSeconds(m_timeBetweenCakes);
            Vector3 position = new Vector3(m_rect.x + Random.Range(m_rect.xMin, m_rect.xMax), 0, m_rect.y + Random.Range(m_rect.yMin, m_rect.yMax));

            if (Physics.SphereCast(position + Vector3.up * 100f,0.62f, Vector3.down, out RaycastHit hit, 150f))
            {
                Instantiate(m_cakePrefab, hit.point + Vector3.up * 0.275f, m_cakePrefab.transform.rotation);
            }

            cakesBrrrd++;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = new Vector3(m_rect.x, 1, m_rect.y);
        Debug.DrawLine(pos + new Vector3(m_rect.xMin,0,m_rect.yMin),pos + new Vector3(m_rect.xMin,0,m_rect.yMax));
        Debug.DrawLine(pos + new Vector3(m_rect.xMax,0,m_rect.yMin),pos + new Vector3(m_rect.xMax,0,m_rect.yMax));
        
        Debug.DrawLine(pos + new Vector3(m_rect.xMin,0,m_rect.yMin),pos + new Vector3(m_rect.xMax,0,m_rect.yMin));
        Debug.DrawLine(pos + new Vector3(m_rect.xMin,0,m_rect.yMax),pos + new Vector3(m_rect.xMax,0,m_rect.yMax));
    }
}
