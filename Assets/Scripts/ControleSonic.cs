using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleSonic : MonoBehaviour
{
    public LayerMask layerMascara;//quais layers vai ter verificação de colisão
    public Vector3 diferenca;
    public float forcaStomp;
    public GameObject barraDeVida;
    public Image iconeVida;
    public int vidaMaxima;
    public Transform respawn;
    private List<Image> Vidas = new List<Image>();
    private Rigidbody2D rb;
    public Animator animator;
    private float posicaoAnterior;
    private const float RAIO = 0.05f;
    private int vidaAtual;
    Vector3 inicio;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        diferenca = new Vector3(0, 0.15f, 0);
        inicio = gameObject.transform.position;
        vidaAtual = vidaMaxima;

        for (int i = 0; i < vidaMaxima; i++)
        {
            Image temp = Instantiate<Image>(iconeVida, barraDeVida.transform) ;
            float largura = temp.rectTransform.rect.width;
            temp.transform.localPosition = new Vector3(largura / 2 + (largura + 3) * i, 0, 0);
            Vidas.Add(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horz = Input.GetAxis("Horizontal");
        posicaoAnterior = transform.position.y;
        if (horz != 0)
        {
            animator.SetBool("CORRENDO", true);
            transform.Translate(0.75f * Time.deltaTime*horz, 0, 0);//anda o personagem direita/esquerda
            if (horz < 0)
                transform.localScale = new Vector3(-1, 1, 1);//vira personagem para esquerda
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            animator.SetBool("CORRENDO", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 4f), ForceMode2D.Impulse);
            animator.SetTrigger("PULAR");
            animator.SetBool("NOCHAO", false);
        }
        if (transform.position.y < posicaoAnterior)
        {
            animator.SetBool("NOCHAO", false);
            animator.SetBool("PISAO", true);
            Stomp();
        }
    }
        private void FixedUpdate()
        {
            Collider2D[] colisoes = Physics2D.OverlapCircleAll(transform.position - diferenca, RAIO, layerMascara);
        if (colisoes.Length == 0)
            animator.SetBool("NOCHAO", false);
        else
        {
            animator.SetBool("NOCHAO", true);
            animator.SetBool("PISAO", false);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - diferenca, RAIO);
    }
    private void Stomp()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("stomp");
            rb.velocity = new Vector2(0f, 0f);
            rb.velocity = new Vector2(0, forcaStomp * -1);
        }
    }
    public void atualizaBarraDeVida(int valor)
    {
        if (valor > 0)//ganhou vida
        {
            if (valor > vidaMaxima)
                valor = vidaMaxima;
            for (int i = 0; i < valor; i++)
            {
                vidaAtual++;
                Image vida = Instantiate(iconeVida, barraDeVida.transform);
                float largura = vida.rectTransform.rect.width;
                vida.transform.localPosition = new Vector3(largura / 2 + (largura + 3) * (Vidas.Count + 1), 0, 0);
                Vidas.Add(vida);
            }
        } 
        else if (valor < 0)//perdeu vida
        {
            if (valor > vidaAtual)
                valor = vidaAtual;
            for (int i = 0; i > valor; i--)
            {
                vidaAtual--;
                Destroy(Vidas[Vidas.Count - 1]);
                Vidas.RemoveAt(Vidas.Count - 1);
            }
            if (vidaAtual <= 0)//morreu
            {
                transform.position = respawn.position;
                atualizaBarraDeVida(vidaMaxima);
            }
        }
    }
}
