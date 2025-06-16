using UnityEngine;

public abstract class EnemyState
{
    protected enemyAI1 ai;

    public EnemyState(enemyAI1 ai)
    {
        this.ai = ai;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }

}
