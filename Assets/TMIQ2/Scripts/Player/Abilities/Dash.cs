using System.Collections;
using System.Collections.Generic;
using Tmiq2.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tmiq2.Abilities
{
    [RequireComponent(typeof(CharacterController))]
    public class Dash : Ability
    {
        [SerializeField] private float dashForce = 30f;
        //TODO: Сейчас это поле не работает на самомо деле, переделать дэш так что бы работал
        //Скорее всего для этого надго будет использовать дельту времени
        [SerializeField] private float dashDuration = 0.2f;
        [Tooltip("Sound played when dashing")]
        public AudioClip dashSFX;

        private PlayerCharacterController playerCC;
        private PlayerInputHandler inputHandler;

        private void Awake()
        {
            playerCC = GetComponent<PlayerCharacterController>();
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        public override IEnumerator Cast()
        {
            playerCC.isDashing = true;
            //Перевод координат в локальные для игрока
            var desiredDirection = transform.TransformVector(inputHandler.GetMoveInput());
            playerCC.characterVelocity = Vector3.Scale(desiredDirection, new Vector3(dashForce, 0f, dashForce));
            // play sound
            playerCC.audioSource.PlayOneShot(dashSFX);
            yield return new WaitForSeconds(dashDuration);
            playerCC.isDashing = false;
            playerCC.characterVelocity = new Vector3(0, 0, 0);
        }

        public void CastDash(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            StartCoroutine(Cast());
        }
    }
}
