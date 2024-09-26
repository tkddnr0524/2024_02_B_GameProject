using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ų Ÿ�� �������̽�
public interface ISkillTarget 
{
    void ApplyEffect(ISkillEffect effect);
    
}

//��ų ȿ�� �������̽�
public interface ISkillEffect 
{
    void Apply(ISkillTarget target);
}

//��ü���� ȿ�� Ŭ����
public class DamageEffect : ISkillEffect
{
    public int Damage { get; private set; }

    public DamageEffect(int damage)
    {
        Damage = damage;
    }

    public void Apply(ISkillTarget target)
    {
        if (target is PlayerTarget playertarget)
        {
            playertarget.Health -= Damage;
            Debug.Log($"Player took {Damage} damage. Remaining health : {playertarget.Health}");
        }
        else if (target is EnemyTarget enemyTarget)
        {
            enemyTarget.Health -= Damage;
            Debug.Log($"Enemy took {Damage} damage. Remaining health : {enemyTarget.Health}");
        }
    }
}

public class HealEffect : ISkillEffect
{
    public int HealAmount { get; private set; }

    public HealEffect(int healAmount)
    {
        HealAmount = healAmount;
    }

    public void Apply(ISkillTarget target)
    {
        if (target is PlayerTarget playertarget)
        {
            playertarget.Health += HealAmount;
            Debug.Log($"Player took {HealAmount} Heal. Remaining health : {playertarget.Health}");
        }
        else if (target is EnemyTarget enemyTarget)
        {
            enemyTarget.Health += HealAmount;
            Debug.Log($"Enemy took {HealAmount} Heal. Remaining health : {enemyTarget.Health}");
        }
    }
}

//���׸� ��ų Ŭ����
public class Skill<TTarget, TEffect>
    where TTarget : ISkillTarget
    where TEffect : ISkillEffect
{
    public string Name { get; private set; }        //��ų �̸�
    public TEffect Effect { get; private set; }     //��ų ȿ��


    public Skill(string name, TEffect effect)
    {
        Name = name;            //�̸��� ������ �־���
        Effect = effect;        //ȿ���� ������ �־���
    }

    public void Use(TTarget target)         //��� (Ÿ���� �����;���)
    {
        Debug.Log($"Using skill: {Name}");
        target.ApplyEffect(Effect);     
    }
}