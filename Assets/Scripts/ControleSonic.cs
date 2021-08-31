using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleSonic : MonoBehaviour
{
    public LayerMask layerMascara;//quais layers vai ter verificação de colisão
    public Vector3 diferenca;
    private Rigidbody2D rb;
    private Animator animator;
    private const float RAIO = 0.05f;
    Vector3 inicio;
    int pulos;
    const int pulosMax = 1;
    [SerializeField]
    int vidas = 3;
    [SerializeField]
    //Text marcadorVidas;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        diferenca = new Vector3(0, 0.15f, 0);
        inicio = gameObject.transform.position;
        pulos = pulosMax;
    }

    // Update is called once per frame
    void Update()
    {
        float horz = Input.GetAxis("Horizontal");

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
        if (Input.GetKeyDown(KeyCode.Space) && pulos>0)
        {
            rb.AddForce(new Vector2(0, 4f), ForceMode2D.Impulse);
            animator.SetTrigger("PULAR");
            animator.SetBool("NOCHAO", false);
            pulos--;
            Debug.Log(pulos);
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
            pulos = pulosMax;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - diferenca, RAIO);
    }

}
