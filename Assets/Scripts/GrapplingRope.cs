using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour {
    private Spring spring;
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;
    public PlayerAndCamera  grapplingGun;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;
    
    void Awake() {
        lr = GameObject.Find("GrappleGun").GetComponent<LineRenderer>();
        grapplingGun = GameObject.Find("Player").GetComponent<PlayerAndCamera>();
        spring = new Spring();
        spring.SetTarget(0);
    }
    
    //Called after Update
    void LateUpdate() 
    {
        DrawRope();
    }

    void DrawRope() 
    {
        if (!grapplingGun.isGrappling) 
        {
            currentGrapplePosition = grapplingGun.gunTip.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0) {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }
        
        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 grapplePoint = grapplingGun.grapplePoint;
        Vector3 gunTipPosition = grapplingGun.gunTip.position;
        Vector3 up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);

        for (var i = 0; i < quality + 1; i++) 
        {
            var delta = i / (float) quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *affectCurve.Evaluate(delta);
            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}
