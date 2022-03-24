using System;
using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PointZoneManager : MonoBehaviour
{
    [SerializeField] private List<TankMovement> tanks = new List<TankMovement>();
    public Slider captureSlider;
    public Image captureSliderImage;
    public float captureLevel;
    public Team currentTeam;
    public Team newTeam;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<TankMovement>())
            tanks.Add(other.GetComponent<TankMovement>());
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<TankMovement>())
            tanks.Remove(other.GetComponent<TankMovement>());
    }

    public List<TankMovement> GetTanks()
    {
        return tanks;
    }

    public int GetTeamAmount()
    {
        int teamAmounts = 0;
        List<Team> teamAlreadyCounted = new List<Team>();
        foreach (var tank in tanks)
        {
            if (!teamAlreadyCounted.Contains(tank.tankTeam))
            {
                teamAlreadyCounted.Add(tank.tankTeam);
                teamAmounts++;
            }
        }

        return teamAmounts;
    }
    private void Update()
    {
        newTeam = FindNewCapturingTeam();
        for (int i = tanks.Count - 1; i >= 0; i--)
        {
            if (!tanks[i].gameObject.activeInHierarchy) tanks.RemoveAt(i);
        }
    }

    public Team FindNewCapturingTeam()
    {
        if (tanks.Count <= 0) return null;
        Team team = tanks[0].tankTeam;
        
        foreach (var tank in tanks)
        {
            if (tank.tankTeam != team)
            {
                return null;
            }
        }
        return team;
    }
}
