using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

/// <summary>
/// �A�j���[�V�����C�x���g����������N���X
/// </summary>
public class AnimationEventHandler 
{
    private Animator m_animator; // �A�j���[�V�����𐧌䂷��Animator�R���|�[�l���g

    private int     m_layerIndex;       // �A�j���[�V�������C���[�̃C���f�b�N�X

    private int m_currentAnimationHash; // ���݂̃A�j���[�V�����̃n�b�V���l

    private bool m_changedLayerWeight = false; // ���C���[�̃E�F�C�g��ύX���邩�ǂ���

    private TargetTimeData m_animationTargetTimeActionData; // �A�j���[�V�������C���[�̕ύX�Ɋւ���f�[�^

    bool m_hasAnimationPlayed = false; // �u���A�j���[�V�������Đ����ꂽ���Ƃ����邩�ǂ����̃t���O

    private string m_paramName; // �A�j���[�V�����̃p�����[�^��

    /// <summary>
    /// �A�j���[�V�������C���[�̕ύX�Ɋւ���f�[�^�\��
    /// </summary>
    public struct TargetTimeData
    {
        public float changedNormalizedTime; // ���C���[�̃E�F�C�g��ύX����A�j���[�V�����̍Đ�����
        public Action action;

    }

    public AnimationEventHandler(Animator animator)
    {
     
        m_animator = animator;

        ResetAnimation(); // �A�j���[�V�����̏�����
    }




    /// <summary>
    /// �A�j���[�V�������Đ����郁�\�b�h
    /// </summary>
    /// <param name="animationName"></param>
    /// <param name="layerIndex"></param>
    public void PlayAnimationTrigger( string paramName, string layerName, string animationName)
    {
        // �A�j���[�V�����̃p�����[�^����ݒ�
        m_paramName = paramName; 

        // �A�j���[�V�������J�n���w��
        m_animator.SetTrigger(paramName);

        // �A�j���[�V�����̍Đ�
        PlayAnimation(animationName, layerName);

    }

    /// <summary>
    /// �A�j���[�V�������Đ����郁�\�b�h�i�u�[���l���g�p�j
    /// </summary>
    /// <param name="boolName"></param>
    /// <param name="layerName"></param>
    /// <param name="animationName"></param>
    public void PlayAnimationBool(string paramName, string layerName, string animationName)
    {
        // �A�j���[�V�����̃p�����[�^����ݒ�
        m_paramName = paramName;

        // �A�j���[�V�������J�n
        m_animator.SetBool(m_paramName, true);

        // �A�j���[�V�����̍Đ�
        PlayAnimation(animationName, layerName);
    }

    public void ResetAnimation()
    {
        m_animationTargetTimeActionData = new TargetTimeData
        {
            changedNormalizedTime = 0.0f,
            action = null
        };

        m_layerIndex = 0;
        m_currentAnimationHash = 0;

        m_hasAnimationPlayed = false; // �u���A�j���[�V�������Đ����ꂽ���Ƃ����邩�ǂ����̃t���O��������

        m_changedLayerWeight = false; // ���C���[�̃E�F�C�g��ύX���邩�ǂ����̃t���O��������

        m_paramName = string.Empty; // �A�j���[�V�����̃p�����[�^����������
    }

    /// <summary>
    /// �w�肵���A�j���[�V�������~���郁�\�b�h
    /// </summary>
    /// <param name="boolName"></param>
    public void StopAnimation()
    {
        // �A�j���[�V�������~
        m_animator.SetBool(m_paramName, false);

    }

    /// <summary>
    /// �w�肵���A�j���[�V�������Đ����郁�\�b�h
    /// </summary>
    /// <param name="animationName"></param>
    /// <param name="layerName"></param>
    private void PlayAnimation(string animationName, string layerName)
    {
        m_layerIndex = m_animator.GetLayerIndex(layerName);
        // �A�j���[�V�������J�n
        m_animator.Play(animationName, m_layerIndex);
        // �A�j���[�V�����̃n�b�V�����擾
        m_currentAnimationHash = Animator.StringToHash(layerName + "." + animationName);
        m_hasAnimationPlayed = false; // �A�j���[�V�������Đ����ꂽ���Ƃ����Z�b�g
    }

    /// <summary>
    /// ���݂̃A�j���[�V�������Đ������ǂ������m�F���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        // ���݂̃A�j���[�V�����̃n�b�V�����擾
        int currentHash = m_animator.GetCurrentAnimatorStateInfo(m_layerIndex).fullPathHash;


        // �A�j���[�V�������Đ�����Ă��邩�ǂ�����҂�
        return (currentHash == m_currentAnimationHash && m_animator.GetCurrentAnimatorStateInfo(m_layerIndex).normalizedTime < 1.0f);
    }


    /// <summary>
    /// �w�肵�����ԂŎ��s����A�N�V������ݒ肷�郁�\�b�h�B
    /// </summary>
    /// <param name="changedNormalizedTime"></param>
    /// <param name="action"></param>
    public void SetTargetTimeAction(float changedNormalizedTime, Action action)
    {
        // ���C���[�̃E�F�C�g��ύX���邩�ǂ����̃t���O�𗧂Ă�
        m_changedLayerWeight = true;
        // �A�j���[�V�����̍Đ��������擾
        m_animationTargetTimeActionData.changedNormalizedTime = changedNormalizedTime;
        m_animationTargetTimeActionData.action = action;
    }

    /// <summary>
    /// �w�肵�����C���[�̃E�F�C�g���A��莞�Ԃ����ĖڕW�l�܂ŕύX����R���[�`���B
    /// </summary>
    public IEnumerator TransitionLayerWeight(int layerIndex, float targetWeight, float duration)
    {
        float startWeight = m_animator.GetLayerWeight(layerIndex);
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newWeight = Mathf.Lerp(startWeight, targetWeight, time / duration);
            m_animator.SetLayerWeight(layerIndex, newWeight);
            yield return null;
        }

        // �Ō�ɃE�F�C�g���m���ɖڕW�l�ɐݒ�
        m_animator.SetLayerWeight(layerIndex, targetWeight);
    }

    /// <summary>
    /// �A�j���[�V�����C�x���g�̍X�V�����B
    /// </summary>
    public void OnUpdate()
    {
        // ���݂̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(m_layerIndex);

        // �A�j���[�V�����̃n�b�V�����擾
        int currentHash = stateInfo.fullPathHash;
        if (currentHash == m_currentAnimationHash)
        {
            m_hasAnimationPlayed = true; // �u���A�j���[�V�������Đ����ł��邱�Ƃ��L�^

        }

        // ���C���[�̃E�F�C�g��ύX����K�v�����邩�m�F
        if (m_changedLayerWeight && IsPlaying() && m_hasAnimationPlayed)
        {
                   // ���C���[�̃E�G�C�g��ς��n�߂�Đ��������擾
            float normalizedTime = m_animationTargetTimeActionData.changedNormalizedTime;

            // �A�j���[�V�����̍Đ��������w�肳�ꂽ�����ɒB���Ă��邩�m�F
            if (stateInfo.normalizedTime >= normalizedTime )
            {
                // �A�j���[�V�����̒������擾
                float animationTotalTime = stateInfo.length;
                // �R���[�`�����J�n
                //StartCoroutine(TransitionLayerWeight(m_layerIndex, m_animationTargetTimeActionData.targetWeight, m_animationTargetTimeActionData.duration));
                // �A�N�V���������s
                m_animationTargetTimeActionData.action.Invoke(); 
            }
        }

    }

    /// <summary>
    /// �A�j���[�V�������Đ����ꂽ���Ƃ����邩�ǂ������m�F���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    public bool HasAnimationPlayed()
    {
        return m_hasAnimationPlayed; // �u���A�j���[�V�������Đ����ꂽ���Ƃ����邩�ǂ�����Ԃ�
    }
}
