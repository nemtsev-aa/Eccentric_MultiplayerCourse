
public class AmmoRocket : Bullet
{
    public override void Ricochet()
    {
        Instantiate(_hitParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
