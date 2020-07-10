using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GFBase : MonoBehaviour {
    /// <summary>
    /// The number of teeth.
    /// On GFGearGen:
    ///     Will be hidden in the inspector when alignTeethWithParent is checked.
    ///     In case alignTeethWithParent is checked, it will calculate the number of teeth based on the radius and the number of teeth of the parent gear.
    /// On GFGear:
	///     The total number of teeth: used to calculate the angle.
	///     Will be hidden in the inspector when there's a GFGearGen component present in the current gameobject.
    /// </summary>
    public int numberOfTeeth = 9;

    /// <summary>
    /// The size of the gear.
    /// Measured from center to outer tip.
    /// </summary>
    public float radius = 1.0f; // -100.0f - 100.0f

    /// <summary>
	/// The length of the tooth.
	/// Will actually be deducted from radius to preserve its size.
	/// </summary>
	public float tipLength = 0.3f;

    /// <summary>
    /// Teeth are on the inside: pointing inwards towards the center of the gear.
    /// </summary>
    public bool innerTeeth = false;

    public bool isShifted { get; set; }

    /// <summary>
    /// Aligns the position and rotation to match gear specified in alignTo.
    /// </summary>
    /// <param name='alignTo'>
    /// Align to.
    /// </param>
    virtual public void Align(GFBase alignTo)
    {
        bool alignToShifted = alignTo.innerTeeth ? !alignTo.isShifted : alignTo.isShifted;

        // ------------------------------------------------------------------------------------------------------------------------------------------------------
        float halfTeethAngle = 180.0f / (float)this.numberOfTeeth;
        float ratio = 1.0f + ((float)alignTo.numberOfTeeth / (float)this.numberOfTeeth);

        if ((float)this.numberOfTeeth % 2.0f == 0.0f)
            this.isShifted = !alignToShifted;
        else
            if (alignTo.numberOfTeeth % 2.0f == 0.0f && alignToShifted)
        {
            this.isShifted = !alignToShifted;
            halfTeethAngle = 0.0f;
        }
        else
            this.isShifted = alignToShifted;
        
        // Determine angle between center of current gear and other gear
        Vector3 directionGlobal = (this.gameObject.transform.position - alignTo.gameObject.transform.position).normalized;
        Vector3 direction = alignTo.transform.worldToLocalMatrix * directionGlobal;
        float headingAngle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);

        //Quaternion initial = new Quaternion(alignTo.gameObject.transform.localRotation.x, alignTo.gameObject.transform.localRotation.y, alignTo.gameObject.transform.localRotation.z, alignTo.gameObject.transform.localRotation.w);

        Vector3 prevRotation = this.gameObject.transform.localRotation.eulerAngles;
        try
        {
//            if (this.innerTeeth)
//            {
//                ratio = 1.0f - ((float)alignTo.numberOfTeeth / (float)this.numberOfTeeth);
//                this.gameObject.transform.localRotation = alignTo.gameObject.transform.localRotation * Quaternion.Euler(prevRotation.x, prevRotation.y, (headingAngle-180f * ratio) - (this.isShifted ? halfTeethAngle : (alignToShifted ? 0.0f : -halfTeethAngle)));
//            }
//            else
//            {
//                if (alignTo.innerTeeth)
//                {
//                    ratio = 1.0f - ((float)alignTo.numberOfTeeth / (float)this.numberOfTeeth);
//                    this.gameObject.transform.localRotation = alignTo.gameObject.transform.localRotation * Quaternion.Euler(prevRotation.x, prevRotation.y, (headingAngle * ratio) - (this.isShifted ? halfTeethAngle : (alignToShifted ? 0.0f : -halfTeethAngle)));
//                }
//                else
                    this.gameObject.transform.localRotation = alignTo.gameObject.transform.localRotation * Quaternion.Euler(prevRotation.x, prevRotation.y, (headingAngle * ratio) - (this.isShifted ? halfTeethAngle : (alignToShifted ? -halfTeethAngle : 0.0f)));
//            }
        }
        catch
        { }
        //if (result.x != float.NaN && result.y != float.NaN && result.z != float.NaN && result.w != float.NaN)
        //	this.gameObject.transform.localRotation = result;
        //}

        // Reposition gear 
        //            if (alignTo.innerTeeth)
        //                this.gameObject.transform.position = alignTo.transform.position + (directionGlobal * ((alignTo.radius - (isShifted ? alignTo.tipLength : 0.0f)) - (this.radius + (isShifted ? tipLength : 0.0f))));
        //          else
        //        this.gameObject.transform.position = alignTo.transform.position + (directionGlobal * ((alignTo.radius - (isShifted ? 0.0f : alignTo.tipLength)) + (this.radius - (isShifted ? tipLength : 0.0f))));


//        if (this.innerTeeth)
//          this.gameObject.transform.position = alignTo.transform.position + (directionGlobal * (this.radius - alignTo.radius - Mathf.Min(this.tipLength, alignTo.tipLength)));
//        else
//            if (alignTo.innerTeeth)
//                this.gameObject.transform.position = alignTo.transform.position + (directionGlobal * (this.radius - alignTo.radius - Mathf.Min(this.tipLength, alignTo.tipLength)));
//            else
                this.gameObject.transform.position = alignTo.transform.position + (directionGlobal * (alignTo.radius + this.radius - Mathf.Min(this.tipLength, alignTo.tipLength)));

    }

}
