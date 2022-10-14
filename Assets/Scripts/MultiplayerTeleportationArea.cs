using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// An area is a teleportation destination which teleports the user to their pointed
/// location on a surface.
/// </summary>
/// <seealso cref="TeleportationAnchor"/>
public class MultiplayerTeleportationArea : BaseTeleportationInteractable
{
    protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        
        Debug.Log(teleportRequest);
        Debug.Log(interactor);
        Debug.Log(raycastHit);

        if (raycastHit.collider == null)
            return false;

        teleportRequest.destinationPosition = raycastHit.point;
        // teleportRequest.destinationRotation = transform.rotation;
        
        return true;
        //
        // return true;
    }
}