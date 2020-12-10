using UnityEngine;

public class CollectButton : MonoBehaviour
{
    //Button should holds all info about particle for better performance
    public Transform[] Target;
    public ParticleSystemForceField[] forceField;
    public Material[] material;
    public FlyingCoin[] coin;

    private bool ParticleAvailable(int index)
    {
        if (index >= coin.Length) return false;
        if (!coin[index].system.isPlaying) return true;
        return false;
    }

    public void Click()
    {
        int j = 0;

        //Loop to check for any free particle to avoid particle change destination when playing
        for (int i = 0; i < coin.Length; i++)
        {
            for (int k = 0; k < coin.Length; k++)
            {
                if (!ParticleAvailable(i))
                {
                    if (i >= coin.Length) return;
                    i += Target.Length;
                }
                else break;
            }

            coin[i].transform.position = transform.position;
            coin[i].Target = Target[j];
            coin[i].system.externalForces.AddInfluence(forceField[j]);
            if (coin[i].system.TryGetComponent(out Renderer renderCom)) //Prevent allocate fake null value
                renderCom.material = material[j];

            coin[i].system.Play();

            if (j >= Target.Length - 1) return; //Should stop increasing value greater than index

            j++;
        }
    }
}
