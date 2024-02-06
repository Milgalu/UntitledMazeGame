using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum OPTIMIZER
{
    Error, GD, SGD, Momentum, Adagrad, RMSProp, Adam, Nadam
}
public class Optimizer
{
    OPTIMIZER opt;
    public float miniSGD(float learningRate, float weight, float gradient) {
        weight -= learningRate* gradient;
		return weight;
	}
    public float momentum(float learningRate, float weight, float gradient, float beta = 0.9f)
    {
        float velocity = 0.0f;
        velocity = beta * velocity + (1.0f - beta) * gradient;
        weight -= learningRate * velocity;
        return weight;
    }
    public float adagrad(float learningRate, float weight, float gradient)
    {
        float epsilon = 1e-7f;
        float squaredGradient = gradient * gradient;
        weight -= (learningRate / (MathF.Sqrt(squaredGradient) + epsilon)) * gradient;
        return weight;
    }
    public float rmsProp(float learningRate, float weight, float gradient, float beta = 0.9f)
    {
        float epsilon = 1e-7f;
        float squaredGradient = 0.0f;
        squaredGradient = beta * squaredGradient + (1.0f - beta) * (gradient * gradient);
        weight -= (learningRate / (MathF.Sqrt(squaredGradient) + epsilon)) * gradient;
        return weight;
    }
    public float adam(float learningRate, float weight, float gradient, float m, float v, int epoch, float beta1 = 0.9f, float beta2 = 0.999f)
    {
        float epsilon = 1e-7f;
    
        m = beta1 * m + (1.0f - beta1) * gradient;
        v = beta2 * v + (1.0f - beta2) * (gradient * gradient);
    
        float m_hat = m / (1.0f - MathF.Pow(beta1, epoch));
        float v_hat = v / (1.0f - MathF.Pow(beta2, epoch));
        weight -= (learningRate / (MathF.Sqrt(v_hat) + epsilon)) * m_hat;
        return weight;
    }
    public float nadam(float learningRate, float weight, float gradient, float m, float v, int epoch, float beta1 = 0.9f, float beta2 = 0.999f)
    {
        float epsilon = 1e-7f;
    
        m = beta1 * m + (1.0f - beta1) * gradient;
        v = beta2 * v + (1.0f - beta2) * (gradient * gradient);
    
        float m_hat = m / (1.0f - MathF.Pow(beta1, epoch));
        float v_hat = v / (1.0f - MathF.Pow(beta2, epoch));
        weight -= (learningRate / (MathF.Sqrt(v_hat) + epsilon)) * (m_hat + beta1 * m_hat);
        return weight;
    }
}
public enum ACTIVATION
{
    Error, None, Sigmoid, ReLU, LeakyReLU, TanH, Maxout, ELU, Softmax
}
public class Activation {
    public float[] ReLU(float[] logits)
    {
        for(int i=0; i<logits.Length; i++)
        {
            logits[i] = MathF.Max(logits[i], 0.0f);
        }
        return logits;
    }
    public float[] Sigmoid(float[] logits)
    {
        for(int i=0; i< logits.Length; i++)
            logits[i] = 1.0f / (1.0f + MathF.Exp(-logits[i]));
        return logits;
    }
    public float[] Softmax(float[] logits)
    {
        float maxLogit = -float.MaxValue;
        for (int i = 0; i < logits.Length; i++)
            if (logits[i] > maxLogit)
                maxLogit = logits[i];
        float expSum = 0.0f;
        for (int i = 0; i < logits.Length; i++)
        {
            logits[i] = MathF.Exp(logits[i] - maxLogit);
            expSum += logits[i];
        }
        for (int i = 0; i < logits.Length; i++)
            logits[i] = logits[i] / expSum;
        
        return logits;
    }
    public float[] TanH(float[] logits)
    {
        for (int i = 0; i < logits.Length; i++)
            logits[i] = MathF.Tanh(logits[i]);
        return logits;
    }
    public float[] LeakyReLU(float[] logits)
    {
        float leak = 0.1f;
        for (int i = 0; i < logits.Length; i++)
            logits[i] = (logits[i] > 0.0f) ? logits[i] : leak * logits[i];
        return logits;
    }
    public float[] ELU(float[] logits)
    {
        float alpha = 0.1f;
        for (int i = 0; i < logits.Length; i++)
            if (logits[i] < 0.0f) logits[i] = alpha * (MathF.Exp(logits[i]) - 1.0f);
        return logits;
    }
}

class FullyConnected
{
    int inputSize;
    int outputSize;
    float[,] weights;
    float[] biases;
    DateTime currentTime;
    int ms;
    public Activation activation = new Activation();
    public Optimizer optimizer = new Optimizer();
    public FullyConnected(int inputSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.outputSize = outputSize;
        weights = new float[outputSize, inputSize];
        biases = new float[outputSize]; // denseSize

        currentTime = DateTime.Now;
        ms = currentTime.Millisecond;
        Initialize();
    }
    private void Initialize()
    {
        for (int i = 0; i < outputSize; i++)
        {
            biases[i] = 0.002473f * (float)ms * (float)(i + 1);
            for (int j = 0; j < inputSize; j++)
            {
                weights[i, j] = 0.001284f * (float)ms * 0.1f * (float)(j + 1) * (float)(i + 1);
            }
        }
    }
    public float[] forward(float[] input)
    {
        float[] output = new float[outputSize];
        float[] act = new float[outputSize];
        for (int i = 0; i < outputSize; i++)
        {
            float neuronOutput = biases[i];
            for (int j = 0; j < inputSize; j++)
            {
                neuronOutput += weights[i, j] * input[j];
            }
            
            output[i] = neuronOutput;

            act = activation.Sigmoid(output);
        }

        return act;
    }
    public void backward(float[] delta, float learningRate, OPTIMIZER opt)
    {
        // OPTIMIZER opt 미구현
        for (int i = 0; i < outputSize; i++)
        {
            biases[i] -= optimizer.miniSGD(learningRate, biases[i], delta[i]);
            for (int j = 0; j < inputSize; j++)
            {
                weights[i, j] = optimizer.miniSGD(learningRate, weights[i, j], delta[i]);
            }
        }
    }
    /*
    public void backward(float[] delta, float loss, float learningRate)
    {
        for(int i=0; i < outputSize; i++)
        {
            biases[i] -= learningRate * delta[i] * loss;
            for(int j=0; j < inputSize; j++) 
            {
                weights[i, j] -= learningRate * delta[i] * loss;
            }
            
        }
    }*/

}

public enum LOSS
{
    MSE, RMSE, Crossentropy
}
class Loss
{
    LOSS lossType;
    public float loss(float target, float output, LOSS lossType)
    {
        float mse = 0.0f;
        float crossEntropy = 0.0f;
        float error = 0.0f;
        float rmse = 0.0f;

        error = target - output;
        mse = error * error;
        rmse = (float)Math.Sqrt(mse);
        crossEntropy = -(target * (float)Math.Log(output + 1e-6f));

        if (lossType == LOSS.MSE)
            return mse;
        else if (lossType == LOSS.RMSE)
            return rmse;
        else if (lossType == LOSS.Crossentropy)
            return crossEntropy;
        else
        {
            Debug.Log("LOSS 타입 에러");
            return -1000.0f;
        }
    }
    public float deLoss(float target, float output, LOSS lossType)
    {
        float deMSE = -target - output;
        float deRMSE = (-target - output) / loss(target, output, LOSS.RMSE);
        float deCE = -(target / (output + 1e-6f));

        if (lossType == LOSS.MSE)
            return deMSE;
        else if (lossType == LOSS.RMSE)
            return deRMSE;
        else if (lossType == LOSS.Crossentropy)
            return deCE;
        else
        {
            Debug.Log("LOSS 타입 에러");
            return -1000.0f;
        }
    }
}

class ActorNetwork
{
    FullyConnected fc;
    Activation act = new Activation();
    private Loss lossValue = new Loss();
    float logProb = 0.0f;
    float[] probabilities;
    public ActorNetwork(int inputSize, int outputSize)
    {
        fc = new FullyConnected(inputSize, outputSize);
    }


    public int SelectAction(List<float> stateSpace)
    {
        float[] stateSpaceToArray = stateSpace.ToArray();
        float[] logits = fc.forward(stateSpaceToArray);

        for(int i=0; i<logits.Length; i++)
        {
            Debug.Log("state " + i + "번째: " + stateSpaceToArray[i]);
            Debug.Log("logit " + i + "번째: " + logits[i]);
        }

        probabilities = act.Softmax(logits);
        float maxPB = -1.0f;
        int maxIndex = -1;
        int actionSpaceSize = 4;

        // action 선택하는 기준
        // 일단 Max값으로 해놓음. probabilities 사이즈랑 denseSize 맞아야됨
        for (int i = 0; i < probabilities.Length; i++)
        {
            if (probabilities[i] > maxPB)
            {
                maxPB = probabilities[i];
                maxIndex = i;
            }
            
        }
        logProb = maxPB;
        int action = maxIndex;

        Debug.Log("액션값: " + action);
        return action;
    }
    /*
    public List<float> CalculateAdvantage(List<State> states, List<Action> actions, List<float> rewards, List<State> nextStates, CriticNetwork criticNetwork)
    {
        List<float> advantages = new List<float>();

        float predictedValue = criticNetwork.EstimateValue(states);
        float nextValue = criticNetwork.Predict(nextStates[i]);
        advantages.Add(rewards[i] + gamma * nextValue - predictedValue);
        
        return advantages;
    }

    public List<float> CalculatePolicyGradient(List<State> states, List<Action> actions, List<float> advantages)
    {
        List<float> policyGradients = new List<float>();
        for (int i = 0; i < states.Count; i++)
        {
            float logProb = GetLogProbability(states[i], actions[i]);
            policyGradients.Add(advantages[i] * logProb);
        }
        return policyGradients;
    }

    public List<float> Clip(List<float> gradients, float epsilon)
    {
        List<float> clippedGradients = new List<float>();
        foreach (float gradient in gradients)
        {
            float clippedGradient = Math.Min(Math.Max(gradient, -epsilon), epsilon);
            clippedGradients.Add(clippedGradient);
        }
        return clippedGradients;
    }
    public float GetLogProbability(State state, Action action) {
        // 상태 벡터를 ActorNetwork 입력으로 변환
        std::vector<float> input = StateToInputVector(state);

        // 신경망 계산 수행
        float log_prob = 0.0f;
        for (int i = 0; i < input.size(); i++) {
            log_prob += weights[i] * input[i];
        }
        float prob = Math.Exp(log_prob);
        return log(prob);
    }    

    public void Update(List<State> states, List<Action> actions, List<float> advantages, float learningRate)
    {
        List<float> policyGradients = CalculatePolicyGradient(states, actions, advantages);
        List<float> clippedGradients = Clip(policyGradients, epsilon);
        // Adam optimizer를 사용하여 ActorNetwork 파라미터 업데이트
        // ...
    }
    */

    public float GetLogProb()
    {
        return this.logProb;
    }
    public void update(float advantage, float learningRate)
    {
        // advantages, 역전파 등
        float deLoss = -this.logProb * advantage;
        for(int i=0; i<probabilities.Length; i++)
        {
            probabilities[i] = probabilities[i] * deLoss;
        }
        fc.backward(this.probabilities, learningRate, OPTIMIZER.SGD);
    }
}
class CriticNetwork
{
    private FullyConnected fc;
    private Loss lossValue = new Loss();
    private float output = 0.0f;
    public CriticNetwork(int inputSize, int outputSize)
    {
        fc = new FullyConnected(inputSize, outputSize);
    }
    public float EstimateValue(float[] state)
    {
        float[] value = fc.forward(state);
        output = value[0];
        return output;
    }
    public void update(float targetValue,float value, float learningRate)
    {
        float criticLoss = lossValue.loss(targetValue, value, LOSS.MSE);
        float criticDeLoss = lossValue.deLoss(criticLoss, output, LOSS.MSE);
        List<float> deLossList = new List<float>();
        deLossList.Add(criticDeLoss);
        fc.backward(deLossList.ToArray(), learningRate,OPTIMIZER.SGD);
    }
}

public enum Move { UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3 };
class Environment
{
    private Vector2 playerPosition, monsterPosition;
    private List<float> stateSpace = new List<float>();
    private List<float> actionSpace = new List<float>();
    private bool isCollision = false;
    public void SetCollision(bool collision) 
    {
        this.isCollision = collision;
    }
    public bool GetCollision()
    {
        return this.isCollision;
    }
    public void SetStateSpace()
    {

        this.playerPosition = GetPlayerPosition();
        this.monsterPosition = GetMonsterPosition();

        this.stateSpace.Clear();
        this.stateSpace.Add(this.playerPosition.x + 0.5f);
        this.stateSpace.Add(this.playerPosition.y + 0.5f);
        this.stateSpace.Add(this.monsterPosition.x * 0.1f);
        this.stateSpace.Add(this.monsterPosition.y * 0.1f);

    }
    public void SetActionSpace(List<float> actionSpace)
    {
        this.actionSpace = actionSpace;
    }
    public List<float> GetStateSpace()
    {
        return this.stateSpace;
    }

    public Vector2 GetPlayerPosition()
    {
        GameObject player = GameObject.Find("Player");
        Vector3 playerPosition3D = player.transform.position;
        Vector2 playerPosition;
        playerPosition.x = playerPosition3D.x;
        playerPosition.y = playerPosition3D.z;
        return playerPosition;
    }
    public Vector2 GetMonsterPosition()
    {
        GameObject monster = GameObject.Find("Monster");
        Vector3 monsterPosition3D = monster.transform.position;
        Vector2 monsterPosition;
        monsterPosition.x = monsterPosition3D.x;
        monsterPosition.y = monsterPosition3D.z;
        return monsterPosition;
    }

    public void UpdateMonsterPosition(float x, float y)
    {
        float step = 0.02f;
        Vector3 movement = new Vector3(x * step, 0, y * step);

        GameObject monster = GameObject.Find("Monster");
        
        if (monster != null)
        {
            /*if (!CheckHitWall(movement))
            {
                monster.transform.position += new Vector3(x * step, 0, y * step);
            }*/
            if (isCollision == false)
            {
                monster.transform.position += new Vector3(x * step, 0, y * step);
            }
        }
        // = new Vector3(x, 0, y) * 0.8f * Time.deltaTime;
    }
    public bool IsEpisodeDone()
    {
        // 충돌하면 에피소드 한번 끝.
        // 몬스터 크기, 플레이어 크기 고려해서 충돌 계산해야함.
        float collision = 1.5f;
        float disX = MathF.Abs(this.playerPosition.x - this.monsterPosition.x);
        float disY = MathF.Abs(this.playerPosition.y - this.monsterPosition.y);
        Debug.Log("캐릭터와 충돌:"+ ((disX < collision) && (disY < collision)));
        return (disX < collision) && (disY < collision);
    }
    /*bool CheckHitWall(Vector3 movement)
    {
        GameObject monster = GameObject.Find("Monster");

        if (monster != null)
        {
            movement = monster.transform.TransformDirection(movement);
            float scope = 1.2f;

            List<Vector3> rayPositions = new List<Vector3>();
            rayPositions.Add(monster.transform.position + Vector3.up * 0.1f);
            rayPositions.Add(monster.transform.position + Vector3.up * boxCollider.size.y * 0.5f);
            rayPositions.Add(monster.transform.position + Vector3.up * boxCollider.size.y);

            foreach (Vector3 pos in rayPositions)
            {
                Debug.DrawRay(pos, movement * scope, Color.red);
            }
            foreach (Vector3 pos in rayPositions)
            {
                if (Physics.Raycast(pos, movement, out RaycastHit hit, scope))
                {
                    if (hit.collider.CompareTag("Wall"))
                        return true;
                }
            }
        }
        return false;
    }*/
}
class Buffer {
    List<List<float>> states;
    List<int> actions;
    List<float> rewards;
    List<float> discountedRewards;
    List<float> logProbs;
    float rewardSum = 0.0f;
    public Buffer()
    {
        states = new List<List<float>>();
        actions = new List<int>();
        rewards = new List<float>();
        discountedRewards = new List<float>();
        logProbs = new List<float>();
    }
    public void StoreTransition(List<float> state, int action, float reward, float logProb)
    {
        states.Add(state);
        actions.Add(action);
        rewards.Add(reward);
        logProbs.Add(logProb);

        rewardSum += reward;

        float maxCilp = 10000.0f;
        float minCilp = -10000.0f;
        if(rewardSum > maxCilp)
        {
            rewardSum = maxCilp;
        }
        if(rewardSum < minCilp)
        {
            rewardSum = minCilp;
        }
    }
    public float GetReward()
    {
        return this.rewardSum;
    }
    public void Clear()
    {
        states.Clear();
        actions.Clear();
        rewards.Clear();
        discountedRewards.Clear();
        logProbs.Clear();
        rewardSum = 0.0f;
    }
    public void CalculateDiscountedRewards(float discountFactor)
    {
        discountedRewards.Clear();
        float cumulativeReward = 0.0f;
        for (int i = rewards.Count - 1; i >= 0; i--)
        {
            cumulativeReward = rewards[i] + discountFactor * cumulativeReward;
            discountedRewards.Insert(0, cumulativeReward);
        }
    }
    public List<float> GetRewards()
    {
        return this.rewards;
    }
    public List<float> GetLogProbs()
    {
        return this.logProbs;
    }
    public List<float> GetDiscountedRewards()
    {
        return this.discountedRewards;
    }
    public List<int> GetActions()
    {
        return this.actions;
    }
    public List<List<float>> GetStates()
    {
        return this.states;
    }
}
public class PPO : MonoBehaviour
{
    private ActorNetwork actorNetwork = new ActorNetwork(4, 4);
    private CriticNetwork criticNetwork = new CriticNetwork(4, 1);
    private Environment env = new Environment();
    private Buffer buffer = new Buffer();
    private float learningRate;
    private float discountFactor;
    private float moveStep = 0.5f;
    private float value = 0.0f;
    private float targetValue = 0.0f;
    private float advantage = 0.0f;
    private bool isMove = true;
    float reward = 0.0f;
    public PPO(float lr, float gamma)
    {
        learningRate = lr;
        discountFactor = gamma;
        // Initialize actor and critic networks
        // Initialize environment

        // Initialize buffer
    }
    public void Train(int numEpisodes)
    {

        buffer.Clear();
        if (!env.IsEpisodeDone())
        {
            var playerPosition = env.GetPlayerPosition();
            var monsterPosition = env.GetMonsterPosition();

            float prevStepX = monsterPosition.x;
            float prevStepY = monsterPosition.y;

            int action = actorNetwork.SelectAction(env.GetStateSpace());
            if (action == (int)Move.UP)
            {
                prevStepY = monsterPosition.y - moveStep; // 이전 스텝은 반대 스텝으로 지정해야됨
                env.UpdateMonsterPosition(monsterPosition.x, monsterPosition.y + moveStep);
            }
            else if (action == (int)Move.DOWN)
            {
                prevStepY = monsterPosition.y + moveStep;
                env.UpdateMonsterPosition(monsterPosition.x, monsterPosition.y - moveStep);
            }
            else if (action == (int)Move.LEFT)
            {
                prevStepX = monsterPosition.x + moveStep;
                env.UpdateMonsterPosition(monsterPosition.x - moveStep, monsterPosition.y);
            }
            else if (action == (int)Move.RIGHT)
            {
                prevStepX = monsterPosition.x - moveStep;
                env.UpdateMonsterPosition(monsterPosition.x + moveStep, monsterPosition.y);
            }

            reward = CalculateReward(playerPosition, monsterPosition);
            if (this.env.GetCollision() == true)
            {
                env.UpdateMonsterPosition(prevStepX, prevStepY);
                reward = -reward; // 벽에 부딪히면 보상을 반대로 줌.

                Debug.Log("벽에 부딪힘");
                this.env.SetCollision(false);
            }



            buffer.StoreTransition(env.GetStateSpace(), action, reward, actorNetwork.GetLogProb());

            // 어드밴티지 및 할인된 보상 계산
            targetValue = reward;
            value = criticNetwork.EstimateValue(env.GetStateSpace().ToArray());
            advantage = CalculateAdvantage(reward, value, discountFactor);
            buffer.CalculateDiscountedRewards(0.99f);
            actorNetwork.update(advantage, this.learningRate);
            criticNetwork.update(targetValue, value, this.learningRate);
        }
    }
    public void SetStateSpace()
    {
        this.env.SetStateSpace();
    }
    public void SetActionSpace(List<float> actionSpace)
    {
        this.env.SetActionSpace(actionSpace);
    }
    public void CollisionWall()
    {
        this.env.SetCollision(true);
    }
    public float distance(float playerX, float playerZ, float monsterX, float monsterZ)
    {
        return MathF.Sqrt(MathF.Pow(monsterX - playerX, 2) + MathF.Pow(monsterZ - playerZ, 2));
    }
    public float CalculateReward(Vector2 playerPosition, Vector2 monsterPosition)
    {
        float minDist = 1.0f;
        float maxReward = 10.0f;
        float maxDist = 130.0f;
        float minReward = -10.0f;
        float reward;
        float dist = distance(playerPosition.x, playerPosition.y, monsterPosition.x, monsterPosition.y);
        if (dist < minDist)
        {
            reward = maxReward;
        }
        else if (dist > maxDist)
        {
            reward = minReward;
        }
        else
        {
            reward = maxReward - (MathF.Log(dist / minDist) / MathF.Log(maxDist / minDist)) * (maxReward - minReward);
            reward *= 1.0f / (dist + 0.1f);
        }

        return reward;
    }
    public float CalculateAdvantage(float reward, float value, float gamma)
    {
        float advantage = 0.0f;
        advantage += gamma * reward;
        advantage += gamma * value - value;
        return advantage;
    }
}