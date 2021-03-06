﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRenderer : MonoBehaviour
{
    public static readonly string[] staticDirections = { "idle_N", "idle_NW", "idle_W", "idle_SW", "idle_S", "idle_SE", "idle_E", "idle_NE" };
    public static readonly string[] runDirections = { "run_N", "run_NW", "run_W", "run_SW", "run_S", "run_SE", "run_E", "run_NE" };
    public static readonly string[] attack1Directions = { "attack1_N", "attack1_NW", "attack1_W", "attack1_SW", "attack1_S", "attack1_SE", "attack1_E", "attack1_NE" };
    public static readonly string[] attack2Directions = { "attack2_N", "attack2_NW", "attack2_W", "attack2_SW", "attack2_S", "attack2_SE", "attack2_E", "attack2_NE" };
    public static readonly string[] dieDirections = { "die_N", "die_NW", "die_W", "die_SW", "die_S", "die_SE", "die_E", "die_NE" };

    Animator animator;
    int lastDirection = 4;

    private void Awake()
    {
        //cache the animator component
        animator = GetComponent<Animator>();
    }

    public void attack1Animation(Vector2 direction)
    {
        lastDirection = DirectionToIndex(direction, 8);
        animator.Play(attack1Directions[lastDirection]);
    }

    public void dieAnimation()
    {
        //lastDirection = DirectionToIndex(direction, 8);
        animator.Play(dieDirections[lastDirection]);
    }

    public void idleAnimation()
    {
        //lastDirection = DirectionToIndex(direction, 8);
        animator.Play(staticDirections[lastDirection]);
    }

    public void attack2Animation(Vector2 direction)
    {
        lastDirection = DirectionToIndex(direction, 8);
        animator.Play(attack2Directions[lastDirection]);
    }

    public void SetDirection(Vector2 direction)
    {

        //use the Run states by default
        string[] directionArray = null;

        //measure the magnitude of the input.
        if (direction.magnitude < .01f)
        {
            //if we are basically standing still, we'll use the Static states
            //we won't be able to calculate a direction if the user isn't pressing one, anyway!
            directionArray = staticDirections;
        }
        else
        {
            //we can calculate which direction we are going in
            //use DirectionToIndex to get the index of the slice from the direction vector
            //save the answer to lastDirection
            directionArray = runDirections;
            lastDirection = DirectionToIndex(direction, 8);
        }

        //tell the animator to play the requested state
        animator.Play(directionArray[lastDirection]);
        //Debug.Log(lastDirection);
    }

    public int getDirection()
    {
        return lastDirection;
    }

    public static int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        //get the normalized direction
        Vector2 normDir = dir.normalized;
        //calculate how many degrees one slice is
        float step = 360f / sliceCount;
        //calculate how many degress half a slice is.
        //we need this to offset the pie, so that the North (UP) slice is aligned in the center
        float halfstep = step / 2;
        //get the angle from -180 to 180 of the direction vector relative to the Up vector.
        //this will return the angle between dir and North.
        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        //add the halfslice offset
        angle += halfstep;
        //if angle is negative, then let's make it positive by adding 360 to wrap it around.
        if (angle < 0)
        {
            angle += 360;
        }
        //calculate the amount of steps required to reach this angle
        float stepCount = angle / step;
        //round it, and we have the answer!
        return Mathf.FloorToInt(stepCount);
    }

    //this function converts a string array to a int (animator hash) array.
    public static int[] AnimatorStringArrayToHashArray(string[] animationArray)
    {
        //allocate the same array length for our hash array
        int[] hashArray = new int[animationArray.Length];
        //loop through the string array
        for (int i = 0; i < animationArray.Length; i++)
        {
            //do the hash and save it to our hash array
            hashArray[i] = Animator.StringToHash(animationArray[i]);
        }
        //we're done!
        return hashArray;
    }
}
