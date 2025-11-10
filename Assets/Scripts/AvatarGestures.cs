using UnityEngine;

public class AvatarGestures : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            anim.SetTrigger("Nod");
        }
    }
}
