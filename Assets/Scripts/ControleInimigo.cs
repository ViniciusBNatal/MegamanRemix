using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleInimigo : MonoBehaviour
{
    [SerializeField]
    private Vector3 raio = new Vector3(10f, 0f, 0f);
    [SerializeField]
    private LayerMask chaoLayer;
    [SerializeField]
    private float velocidade;
    float largura;
    float altura;
    float sentido = 1;
    public int dano = 1;
    // Start is called before the first frame update
    void Start()
    {
        largura = GetComponent<SpriteRenderer>().bounds.size.x;
        altura = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(raio.normalized * Time.deltaTime * velocidade * sentido);
        Vector3 posicao = transform.position + new Vector3(largura * sentido, 0f, 0f);
        RaycastHit2D[] colisao = Physics2D.BoxCastAll(posicao, new Vector2(largura, altura), 0f, new Vector2(sentido, 0), 0f, chaoLayer);
        if (colisao.Length < 0)
            sentido = -sentido;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<ControleSonic>().animator.GetBool("PISAO"))
            {
                Destroy(this.gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<ControleSonic>().atualizaBarraDeVida(dano);
            }
        }
    }
}
