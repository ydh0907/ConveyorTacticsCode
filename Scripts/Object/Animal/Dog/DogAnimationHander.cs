using Unity.Cinemachine;
using UnityEngine;

public class DogAnimationHander : MonoBehaviour
{
    [SerializeField] private Animal owner;
    [SerializeField] private DogShootGun shootGun;
    [SerializeField] private DogRide ride;
    [SerializeField] private CinemachineImpulseSource shootGunShake;
    [SerializeField] private CinemachineImpulseSource rideShake;

    public void Destroy()
    {
        Destroy(owner.gameObject);
    }

    public void Shoot()
    {
        shootGun.CastDamage();
        shootGunShake.GenerateImpulse();
    }

    public void JumpAttack()
    {
        ride.CastDamage();
        rideShake.GenerateImpulse();
    }
}
