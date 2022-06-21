using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitEnemy : MonoBehaviour
{
    private Rigidbody2D m_rig;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        m_rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_rig.velocity = new Vector2(speed, m_rig.velocity.y);
        Physics2D.IgnoreLayerCollision(9, 9);

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag =="Piso")
        {
            speed *= -1;
            this.transform.localScale = new Vector2(this.transform.localScale.x * -1, this.transform.localScale.y);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Pared")
        {
            speed *= -1;
            this.transform.localScale = new Vector2(this.transform.localScale.x * -1, this.transform.localScale.y);
        }
    }
}
