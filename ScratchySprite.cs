﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScratchySprite : ScratchyObject
{

    public Sprite[] Costumes;

    protected SpriteRenderer spriteRenderer;

    private int costumeNumber = -1;
    private int CollisionAccuracy = 1;

    internal static Dictionary<Type, List<ScratchySprite>> Instances = new Dictionary<Type, List<ScratchySprite>>();

    /// <summary>
    /// Initialization
    /// </summary>
    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        NextCostume();
    }

    public float Rotate(float degrees)
    {
        this.transform.Rotate(0, 0, degrees);
        return this.transform.rotation.z;
    }

    private void RegisterInstance()
    {
        Type t = this.GetType();
        if (!Instances.ContainsKey(t))
        {
            Instances[t] = new List<ScratchySprite>();
        }
        if (!Instances[t].Contains(this))
        {
            Instances[t].Add(this);
        }
    }

    private void UnregisterInstance()
    {
        Type t = this.GetType();
        if (Instances[t].Contains(this))
        {
            Instances[t].Remove(this);
        }
    }

    public void OnEnable()
    {
        RegisterInstance();
    }

    public void OnDisable()
    {
        UnregisterInstance();
    }

    public void OnDestroy()
    {
        UnregisterInstance();
    }

    /// <summary>
    /// Switch to the next costume
    /// </summary>
    public void NextCostume()
    {
        if (Costumes.Length == 0)
        {
            return;
        }

        costumeNumber++;
        if (costumeNumber >= Costumes.Length)
        {
            costumeNumber = 0;
        }
        SetCostume(costumeNumber);
    }

    /// <summary>
    /// Gets or sets the costume number
    /// </summary>
    public int CostumeNumber
    {
        get
        {
            return costumeNumber;
        }
        set
        {
            SetCostume(value);
        }
    }

    /// <summary>
    /// Set the costume numberr
    /// </summary>
    /// <param name="costumeNumber">The costume number (0 based)</param>
    public void SetCostume(int costumeNumber)
    {
        this.costumeNumber = costumeNumber % this.Costumes.Length;
        var costume = Costumes[CostumeNumber];
        if (costume != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = costume;
        }
    }

    /// <summary>
    /// Show this ScratchySprite if it has been hidden
    /// </summary>
    public void Show()
    {
        this.spriteRenderer.enabled = true;
    }

    /// <summary>
    /// Hide this ScratchySprite
    /// </summary>
    public void Hide()
    {
        this.spriteRenderer.enabled = false;
    }

    /// <summary>
    /// X position in world space
    /// </summary>
    public double X
    {
        get
        {
            return this.transform.position.x;
        }

        set
        {
            this.transform.position = new Vector3((float)value, transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// Y position in world space
    /// </summary>
    public double Y
    {
        get
        {
            return this.transform.position.y;
        }

        set
        {
            this.transform.position = new Vector3(transform.position.x, (float)value, transform.position.z);
        }
    }

    /// <summary>
    /// Z position in world space
    /// </summary>
    public double Z
    {
        get
        {
            return this.transform.position.z;
        }

        set
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, (float)value);
        }
    }

    /// <summary>
    /// Z rotation
    /// </summary>
    public float Rotation
    {
        get
        {
            // todo: not right
            return this.transform.rotation.eulerAngles.z;
        }

        set
        {
            this.transform.rotation = Quaternion.Euler(0, 0, value);
        }
    }

    /// <summary>
    /// Z direction
    /// </summary>
    public float Direction
    {
        get
        {
            return direction.HasValue ? direction.Value : this.Rotation;
        }

        set
        {
            direction = value;
        }
    }
    private float? direction;

    /// <summary>
    /// Destroy this ScratchySprite instance
    /// </summary>
    public void Destroy()
    {
        UnityEngine.Object.Destroy(this.gameObject);
    }

    /// <summary>
    /// Gets the sprite renderer for this ScratchySprite
    /// </summary>
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            return _spriteRenderer;
        }
    }
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Get the bounds of this sprite in world space
    /// </summary>
    /// <returns>Bounds of this sprite in world space</returns>
    public Bounds GetWorldBounds()
    {
        return this.SpriteRenderer.bounds;

        /* The long way -->
        Bounds bounds = this.SpriteRenderer.sprite.bounds;
        Vector3 c1 = this.transform.TransformPoint(bounds.min);
        Vector3 c2 = this.transform.TransformPoint(bounds.max);
        Vector3 c3 = this.transform.TransformPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z));
        Vector3 c4 = this.transform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z));
        // Debug.DrawLine(c1, c2, Color.gray);
        Bounds transformedBounds = new Bounds();
        transformedBounds.SetMinMax(
            new Vector3(
                Mathf.Min(c1.x, Mathf.Min(c2.x, Mathf.Min(c3.x, c4.x))),
                Mathf.Min(c1.y, Mathf.Min(c2.y, Mathf.Min(c3.y, c4.y))), 0),
            new Vector3(
                Mathf.Max(c1.x, Mathf.Max(c2.x, Mathf.Max(c3.x, c4.x))),
                Mathf.Max(c1.y, Mathf.Max(c2.y, Mathf.Max(c3.y, c4.y))), 0));

        Debug.Log("TB: " + transformedBounds + "  RB:" + this.SpriteRenderer.bounds); 
        return transformedBounds;
        */
    }

    /// <summary>
    /// Get the bounds of this sprite in world space
    /// </summary>
    /// <param name="c"></param>
    public void DrawWorldBounds(Color c)
    {
        Bounds bt = GetWorldBounds();
        DrawRectangle(bt, c);
    }

    /// <summary>
    /// Find a ScratchySprite with the given name
    /// </summary>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public ScratchySprite FindSprite(string spriteName)
    {
        var go = GameObject.Find(spriteName);
        if (go != null)
        {
            return go.GetComponent<ScratchySprite>();
        }
        return null;
    }

    /// <summary>
    /// Get the color of the pixel for this sprite at the given coordinates in sprite local space
    /// </summary>
    /// <param name="objectX">X position in sprite local space</param>
    /// <param name="objectY">Y position in sprite local space</param>
    /// <returns>Color of the pixel</returns>
    private Color32 GetPixel(float objectX, float objectY)
    {
        Rect tr = this.SpriteRenderer.sprite.textureRect;
        var pivotPointOffset = new Vector3(tr.center.x - tr.xMin, tr.center.y - tr.yMin, 0);
        return this.SpriteRenderer.sprite.texture.GetPixel((int)(tr.x + pivotPointOffset.x + objectX), (int)(tr.y + pivotPointOffset.y + objectY));
    }

    /// <summary>
    /// Get all of the sprites of a given type that are touching this sprite
    /// </summary>
    /// <typeparam name="T">Type of sprite to check</typeparam>
    /// <returns>List of touching sprites</returns>
    public List<T> GetTouchingSprites<T>() where T : ScratchySprite
    {
        List<T> touching = new List<T>();
        foreach (var sprite in GetSprites<T>())
        {
            if (IsTouching(sprite))
            {
                touching.Add((T)sprite);
            }
        }
        return touching;
    }

    /// <summary>
    /// Get a single touching sprite of a given type
    /// </summary>
    /// <typeparam name="T">Type of sprite to check</typeparam>
    /// <returns>The touching sprite object, or null if none are touching</returns>
    public T GetTouchingSprite<T>() where T : ScratchySprite
    {
        foreach (var sprite in GetSprites<T>())
        {
            if (IsTouching(sprite))
            {
                return (T)sprite;
            }
        }
        return null;
    }

    /// <summary>
    /// Are any sprites of the given type touching this sprite
    /// </summary>
    /// <typeparam name="T">The type of sprite to check</typeparam>
    /// <returns>True if any are touching, otherwise False</returns>
    public bool IsTouchingAny<T>() where T : ScratchySprite
    {
        foreach (var sprite in GetSprites<T>())
        {
            if (IsTouching(sprite))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Is this sprite touching the other sprite (pixel perfect collission)
    /// </summary>
    /// <param name="other">The other sprite object to check</param>
    /// <returns>True if they are touching</returns>
    public bool IsTouching(ScratchySprite other)
    {
        if (other == null)
        {
            return false;
        }

        Bounds thisWorldBounds = this.GetWorldBounds();
        Bounds otherWorldBounds = other.GetWorldBounds();
        bool collision = thisWorldBounds.Intersects(otherWorldBounds);

        // World bounding rects intersect, now check pixels
        if (collision)
        {
            collision = false;
            Bounds worldIntersection = thisWorldBounds.Intersect(otherWorldBounds);
            Bounds thisSpriteBounds = this.SpriteRenderer.sprite.bounds;
            Bounds otherSpriteBounds = other.SpriteRenderer.sprite.bounds;
            Vector3 pixelWorldPos;
            Color32 c;
            for (float x = thisSpriteBounds.min.x + 0.5f; x < thisSpriteBounds.max.x && !collision; x += CollisionAccuracy)
            {
                for (float y = thisSpriteBounds.min.y + 0.5f; y < thisSpriteBounds.max.y && !collision; y += CollisionAccuracy)
                {
                    pixelWorldPos = this.transform.TransformPoint(new Vector3(x, y, 0));
                    if (worldIntersection.Contains(pixelWorldPos))
                    {
                        c = GetPixel(x, y);
                        if (c.a > 0)
                        {
                            // Get this pixel position in other sprite
                            Vector3 otherLocalPos = other.transform.InverseTransformPoint(pixelWorldPos);
                            if (otherSpriteBounds.Contains(otherLocalPos))
                            {
                                if (other.GetPixel(otherLocalPos.x, otherLocalPos.y).a > 0)
                                {
                                    collision = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return collision;
    }

    /// <summary>
    /// Is the sprite touching or outside of the camera's view
    /// </summary>
    /// <returns>True if any corner of the sprite is outside of the camera's view</returns>
    public bool IsTouchingEdge()
    {
        Bounds cameraBounds = Camera.main.OrthographicBounds();
        Bounds spriteBounds = this.GetWorldBounds();
        return !cameraBounds.Contains(spriteBounds.min) || !cameraBounds.Contains(spriteBounds.max)
            || !cameraBounds.Contains(new Vector3(spriteBounds.min.x, spriteBounds.max.y, 0))
            || !cameraBounds.Contains(new Vector3(spriteBounds.max.x, spriteBounds.min.y, 0));
    }

    /// <summary>
    /// Move some distance in either the current direction or a given direction
    /// </summary>
    /// <param name="distance">Distance to move</param>
    /// <param name="direction">Direction to move, or null to use the sprites direction or rotation</param>
    public void Move(float distance, float? direction = null)
    {
        float dir;
        if (direction.HasValue)
        {
            dir = direction.Value;
        }
        else
        {
            dir = this.Direction;
        }

        var d = Quaternion.Euler(0, 0, dir) * Vector3.up;

        this.transform.Translate(d * distance, Space.World);
    }

}
