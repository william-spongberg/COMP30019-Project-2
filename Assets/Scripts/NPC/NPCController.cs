using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

// need ai agent and animator
[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class NPCController : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.0f;
    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Vector3 dest = Vector3.zero;

    private Animator anim;
    private NavMeshAgent agent;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator> ();
        agent = GetComponent<NavMeshAgent> ();

        // animation multiplier
        anim.SetFloat("MotionSpeed", 0.25f);
    }

    void Update()
    {
        // make nav agent move towards player
        agent.destination = player.transform.position;
        dest = agent.destination;
        velocity = agent.velocity;
        speed = agent.velocity.magnitude;
        
        // let animation know agent speed
        anim.SetFloat("Speed", speed);
    }

    // ? avoids annoying error messages
    private void OnFootstep(AnimationEvent animationEvent)
    {
        // do nothing for now
        // TODO: play footstep sound
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        // do nothing for now
        // TODO: play landing sound
    }
}