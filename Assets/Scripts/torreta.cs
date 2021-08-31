using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torreta : MonoBehaviour
{
    public float alcance;
    public float taxaDeDisparo;
    public float forca;
    float proximoTempoDeDisparo = 0;
    bool detectado = false;
    Vector2 direcao;
    public GameObject luzAlarme;
    public GameObject arma;
    public GameObject bala;
    public Transform pontoDeDisparo;
    public Transform alvo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 alvopos = alvo.position;
        direcao = alvopos - (Vector2)transform.position;
        RaycastHit2D rayinfo = Physics2D.Raycast(transform.position, direcao, alcance);
        
        if (rayinfo)
        {
            if (rayinfo.collider.gameObject.tag == "Player")
            {
                if (!detectado)
                {
                    detectado = true;
                    luzAlarme.GetComponent<SpriteRenderer>().color = Color.red;
                    Debug.Log("entrou");
                }
            }
            else
            {
                if (detectado)
                {
                    detectado = false;
                    luzAlarme.GetComponent<SpriteRenderer>().color = Color.green;
                    Debug.Log("saiu");
                }
            }
        }
        if (detectado)
        {
            arma.transform.up = direcao * -1;
            if(Time.time > proximoTempoDeDisparo)
            {
                proximoTempoDeDisparo = Time.time + 1 / taxaDeDisparo;
                atira();
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, alcance);
    }

    private void atira()
    {
        GameObject balains = Instantiate(bala, pontoDeDisparo.position, Quaternion.identity);
        balains.GetComponent<Rigidbody2D>().AddForce(direcao * forca);
    }
}
