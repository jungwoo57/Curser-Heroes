using System.Collections;
using UnityEngine;

public class TestCursorMove : MonoBehaviour
{
        [Header("Movement Settings")]
        [Range(0, 1f)]
        public float cursorSpeed = 1f;

        private float originalSpeed;   
        private bool isStunned;
    

        private void Awake()
        {
            originalSpeed = cursorSpeed;
            transform.position = Vector3.zero;
        }

        private void Start()
        {
            
        }
        private void Update()
        {
            //if (!isStunned)
                // MouseMoving();
            //float moveX = Input.GetAxis("Mouse X") * cursorSpeed; 
            //float moveY = Input.GetAxis("Mouse Y") * cursorSpeed;
            float moveX = Input.GetAxis("Mouse X");
            float moveY = Input.GetAxis("Mouse Y");
            transform.position += new Vector3(moveX, moveY, 0f);
        }

        void MouseMoving()
        {
            Vector2 objectPos = transform.position;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)
            );
            if (Vector2.Distance(objectPos, mousePos) > 1.0f)
                transform.position = Vector2.Lerp(objectPos, mousePos, cursorSpeed*Time.deltaTime);
        }

    
        public void Stun(float duration)
        {
            if (isStunned) return;
            StartCoroutine(StunCoroutine(duration));
        }

        private IEnumerator StunCoroutine(float duration)
        {
            isStunned = true;

      
            float prevSpeed = cursorSpeed;
            cursorSpeed = 0f;

            yield return new WaitForSeconds(duration);

       
            cursorSpeed = prevSpeed;
            isStunned = false;
        }
        
    }

