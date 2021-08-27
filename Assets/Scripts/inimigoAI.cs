using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class inimigoAI : MonoBehaviour
{
    public Transform alvo;
    public Transform inimigoVisual;
    public float velocidade = 1;
    public float proximaDistanciaPontoDePatrulha = 3f;
    Path path;
    int pontoDePatrulhaAtual = 0;
    bool chegouFimPatrulha = false;
    Seeker seeker;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("AtualizaPath", 0f, .5f);
    }
    void AtualizaPath()
    {
        if(seeker.IsDone())
        seeker.StartPath(rb.position, alvo.position, AoCompletarPath);
    }
    void AoCompletarPath(Path p)
    {
        if (!p.error)
        {
            path = p;
            pontoDePatrulhaAtual = 0;
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;
        if (pontoDePatrulhaAtual >= path.vectorPath.Count)
        {
            chegouFimPatrulha = true;
            return;
        }
        else
            chegouFimPatrulha = false;
        Vector2 direcao = ((Vector2)path.vectorPath[pontoDePatrulhaAtual] - rb.position).normalized;
        Vector2 forca = direcao * velocidade * Time.deltaTime;
        float distancia = Vector2.Distance(rb.position, path.vectorPath[pontoDePatrulhaAtual]);

        rb.AddForce(forca);

        if (distancia < proximaDistanciaPontoDePatrulha)
            pontoDePatrulhaAtual++;

        if (forca.x >= 0.01f)
        {
            inimigoVisual.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (forca.x <= -0.01f)
        {
            inimigoVisual.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
