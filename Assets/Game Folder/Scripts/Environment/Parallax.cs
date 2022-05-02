using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject m_CameraPlayer;
    [SerializeField] private float m_SpeedParallax;
    float m_Length;
    float m_StartPosition;

    void Start()
    {
        m_StartPosition = transform.position.x;
        m_Length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    
    void FixedUpdate()
    {
        float temp = m_CameraPlayer.transform.position.x * (1 - m_SpeedParallax);
        float distance = m_CameraPlayer.transform.position.x * m_SpeedParallax;

        transform.position = new Vector3(m_StartPosition + distance, transform.position.y, transform.position.z);

        if (temp > m_StartPosition + m_Length)
        {
            m_StartPosition += m_Length;
        } else if(temp < m_StartPosition + m_Length)
        {
            m_StartPosition -= m_Length;
        }
    }
}
