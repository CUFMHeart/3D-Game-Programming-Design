using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  用于存储粒子的半径及偏角信息
public class ParticleData
{
    public float radius = 0;
    public float angle = 0;
    public ParticleData(float r, float a)
    {
        this.radius = r;
        this.angle = a;
    }
}
//  MyParticle - 粒子系统的设置
public class MyParticle : MonoBehaviour
{
    //  粒子系统 与 粒子数组
    public int particle_num = 10000;
    private ParticleSystem particle_system; 
    private ParticleData[] particle_data_array;
    private ParticleSystem.Particle[] particle_array;
    //  init
    void Start ()
    {
        float min_radius = 2.0f;
        float max_radius = 3.5f;
        particle_system = this.GetComponent<ParticleSystem>();
        particle_data_array = new ParticleData[particle_num];
        particle_array = new ParticleSystem.Particle[particle_num];
        particle_system.startSpeed = 0;   
        //  设置最大粒子数 
        particle_system.maxParticles = particle_num;
        //  设置为无循环
        particle_system.loop = false;
        //  粒子发射
        particle_system.Emit(particle_num);
        particle_system.GetParticles(particle_array);
        //  粒子初始化
        for (int i = 0; i < particle_num; i++)
        {
            //  得到每个粒子的大小、运动半径和偏角
            //  半径应使得粒子概率分布于周围，且集中于平均半径附近
            float size = Random.Range(0.01f, 0.02f);
            float min_radius_rate = Random.Range(1.0f, (max_radius + min_radius) / 2 / min_radius);
            float max_radius_rate = Random.Range((max_radius + min_radius) / 2 / max_radius, 1.0f);
            float radius = Random.Range(min_radius * min_radius_rate, max_radius * max_radius_rate);
            float angle = Random.Range(0, 2 * Mathf.PI);
            //  对应到粒子数组
            particle_data_array[i] = new ParticleData(radius, angle);            
            particle_array[i].size = size;  
            particle_array[i].position = new Vector3(particle_data_array[i].radius * Mathf.Cos(angle), 0f, particle_data_array[i].radius * Mathf.Sin(angle));
        }
        //  对应到粒子系统
        particle_system.SetParticles(particle_array, particle_array.Length);
    }
    //  update
    void Update()
    {
        for (int i = 0; i < particle_num; i++)
        {
            //  使粒子的半径时刻发生微小变化
            float offset = Random.Range(-0.01f, 0.01f);
            particle_data_array[i].radius += offset;
            //  使粒子的旋转速度时刻发生变化
            float angle = Random.Range(0, 2 * Mathf.PI);
            //  对应到粒子数组
            particle_array[i].position = new Vector3(particle_data_array[i].radius * Mathf.Cos(angle), 0f, particle_data_array[i].radius * Mathf.Sin(angle));
        }
        //  对应到粒子系统
        particle_system.SetParticles(particle_array, particle_array.Length);
    }
}