using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraficoContoller : MonoBehaviour
{
    public Graficonodirigido nodo; 
    public float speed; 
    public int energy; 
    public int maxEnergy = 10; 
    public float descansoTiempo = 5f; 

    private Rigidbody2D rb;
    private Vector2 dir;
    private bool isdescansar;
    private float tiempo;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetNewDireccion();
        energy -= nodo.cost_energy;
    }

    private void Update()
    {
        if (isdescansar)
        {
            tiempo += Time.deltaTime;
            if (tiempo >= descansoTiempo)
            {
                tiempo = 0;
                energy = Mathf.Min(maxEnergy, energy + 1);
                if (energy >= nodo.cost_energy)
                {
                    isdescansar = false;
                    SetNewDireccion();
                }
            }
            return;
        }

        rb.velocity = dir.normalized * speed * Time.deltaTime;

        if (Vector2.Distance(nodo.transform.position, transform.position) < 0.1f)
        {
            rb.velocity = Vector2.zero;
            CheckNodo();
        }
    }

    private void CheckNodo()
    {
        Graficonodirigido node_tmp = nodo.GetNodeRandom();
        if (energy >= node_tmp.cost_energy)
        {
            nodo = node_tmp;
            energy -= nodo.cost_energy;
            SetNewDireccion();
        }
        else
        {
            StartCoroutine(descansaryrecuperacion());
        }
    }

    private void SetNewDireccion()
    {
        dir = (Vector2)nodo.transform.position - rb.position;
    }

    private IEnumerator descansaryrecuperacion()
    {
        isdescansar = true;
        rb.velocity = Vector2.zero;
        tiempo = 0;

        while (isdescansar)
        {
            yield return new WaitForSeconds(5f); 
            energy++;
            if (energy >= nodo.cost_energy)
            {
                isdescansar = false;
                SetNewDireccion();
            }
        }
    }
}
