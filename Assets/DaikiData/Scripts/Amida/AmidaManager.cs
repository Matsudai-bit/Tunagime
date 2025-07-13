
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ����
/// </summary>
public enum Direction
{
    UP,
    DOWN,
    CENTER,
    RIGHT,
    LEFT
}

/// <summary>
/// ���݂������̊Ǘ��N���X�B���݂������̃p�C�v������ړ������A�F�̕ύX�Ȃǂ��Ǘ����܂��B
/// </summary>
public class AmidaManager : MonoBehaviour
{
    private GameObject[,] m_amidaTubeGrid;   // ���݂��̃O���b�h

    public MapData m_map; // �}�b�v�f�[�^

    public Material m_amidaBaseMaterial; // ��{�}�e���A��
    public Material m_amidaElectricityMaterial; // �d�C�̃}�e���A��
    public Material m_amidaRedElectricityMaterial; // �Ԃ��d�C�̃}�e���A��

    // �e�N�X�`��
    public Texture m_baseTexture;
    public Texture m_blueTexture;
    public Texture m_greenTexture;

    private GameObject[,] m_prevAmidaGimmickGrid; // �O��̂��݂��M�~�b�N�O���b�h
    private GameObject[,] m_prevTopGimmickGrid; // �O��̃g�b�v�M�~�b�N�O���b�h

    // ���݂��̈ړ��L���b�V��
    struct AmidaMoveCash
    {
        public GridPos gridPos; // �O���b�h�ʒu
        public Direction movementDir; // �ړ�����
        public Electric.ElectricFlowType electricFlowType; // �d�C�̎��
    }

    private List<AmidaMoveCash> m_amidaMoveCash = new List<AmidaMoveCash>(); // ���݂��̈ړ��L���b�V�����X�g
    private List<AmidaMoveCash> m_amidaPrevMoveCash = new List<AmidaMoveCash>(); // �O��̂��݂��̈ړ��L���b�V�����X�g

    private bool m_startAmida = false; // ���݂��̊J�n�t���O

    private void Awake()
    {
        
    }

    /// <summary>
    /// �Q�[���J�n���̏����������B���݂��`���[�u�̍쐬���s���܂��B
    /// </summary>
    void Start()
    {
        // ���̃O���b�h���擾
        m_prevAmidaGimmickGrid = new GameObject[m_map.GetCommonData().height, m_map.GetCommonData().width];
        m_prevTopGimmickGrid = new GameObject[m_map.GetCommonData().height, m_map.GetCommonData().width];

    }

    /// <summary>
    /// �X�V�����B
    /// </summary>
    void Update()
    {
        //// ���݂��O���b�h�����ݒ�̏ꍇ�A�ݒ肷��
        //if (m_amidaTubeGrid == null)
        //    m_amidaTubeGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.AMIDA);

        //// ���݂��̊J�n�t���O�������Ă���ꍇ�A���݂������Z�b�g���čĊJ�n����
        //if (m_startAmida)
        //{
        //    ResetAllBlockColor();
        //    foreach (var cash in m_amidaMoveCash)
        //    {
        //        StartAmidakuji(cash.gridPos, cash.electricFlowType, cash.movementDir);
        //    }
        //    m_startAmida = false;
        //}

        //// ���̃O���b�h���擾
        //GameObject[,] originalTopGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.TOP);
        //GameObject[,] originalAmidaGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.AMIDA);

        //// �O���b�h�̓��e���r���A�ύX������΃R�s�[
        //for (int i = 0; i < originalAmidaGrid.GetLength(0); i++)
        //{
        //    for (int j = 0; j < originalAmidaGrid.GetLength(1); j++)
        //    {
        //        if (m_prevAmidaGimmickGrid[i, j] != originalAmidaGrid[i, j] ||
        //            m_prevTopGimmickGrid[i, j] != originalTopGrid[i, j])
        //        {
        //            m_startAmida = true;
        //            m_prevTopGimmickGrid[i, j] = originalTopGrid[i, j];
        //            m_prevAmidaGimmickGrid[i, j] = originalAmidaGrid[i, j];
        //        }
        //    }
        //}

        //// ���݂��̊J�n�t���O�������Ă���ꍇ�A�O��̈ړ��L���b�V����ۑ����A���݂̈ړ��L���b�V�����N���A����
        //if (m_startAmida)
        //    m_amidaPrevMoveCash = new List<AmidaMoveCash>(m_amidaMoveCash);

        //m_amidaMoveCash.Clear();
    }

    /// <summary>
    /// ���݂��`���[�u�̒ǉ��v��
    /// </summary>
    /// <param name="gridPos">�O���b�h�ʒu</param>
    /// <param name="amidaTubeObject">���݂��`���[�u�I�u�W�F�N�g</param>
    /// <returns>true ���� false ���s</returns>
    public bool RequestAddGridAmidaTube(GridPos gridPos, GameObject amidaTubeObject)
    {
        //m_amidaTubeGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.AMIDA);

        //// �R���|�[�l���g�������Ă��Ȃ��ꍇ�͈ȉ��̏������΂�
        //if (amidaTubeObject.GetComponent<AmidaTube>() == false) return false;
        //// �O���b�h�͈͓̔��łȂ���Έȉ��̏������΂�
        //if (m_map.CheckInnerGridPos(gridPos) == false) return false;
        //// �O���b�h�ɑ��̃I�u�W�F�N�g�������Ă���Έȉ��̏������΂�
        //if (m_amidaTubeGrid[gridPos.y, gridPos.x] != null)
        //{
        //    return false;
        //    //amidaTubeObject.SetActive(false);
        //}

        //AmidaTube amidaTube = amidaTubeObject.GetComponent<AmidaTube>();

        //// �ǉ�����
        //m_amidaTubeGrid[gridPos.y, gridPos.x] = amidaTubeObject;

        ////m_amidaTubeGrid = m_map.GetFloorManager().SetGimmickFloor(FloorManager.FloorType.AMIDA, m_amidaTubeGrid);


        //// �ʉߕ�����ݒ�
        //SetTubeConnections(gridPos, amidaTube.GetDirectionPassage());

        return true;
    }

    /// <summary>
    /// �w�肵���O���b�h�̃f�[�^���폜����
    /// </summary>
    /// <param name="gridPos">�O���b�h�ʒu</param>
    /// <returns>true ���� false ���s</returns>
    public bool RequestRemoveGridAmidaTube(GridPos gridPos)
    {
        // �O���b�h�͈͓̔��łȂ���Έȉ��̏������΂�
        if (m_map.CheckInnerGridPos(gridPos) == false) return false;

        m_amidaTubeGrid[gridPos.y, gridPos.x].SetActive(true);

        m_amidaTubeGrid[gridPos.y, gridPos.x] = null;
        // ���݂����폜�������߂ǂ̕����������Ă��Ȃ����̂��쐬
        AmidaTube.DirectionPassage initPassage = new AmidaTube.DirectionPassage();

        // �ʉߕ�����������
        SetTubeConnections(gridPos, initPassage);

        return true;
    }

    /// <summary>
    /// �w�肳�ꂽ�O���b�h���W�̒ʘH�����Ŋe�ʉߕ�����ݒ肷��֐�
    /// </summary>
    /// <param name="gridPos">��O���b�h���W</param>
    /// <param name="checkDirectionPassage">�ʉߕ���</param>
    private void SetTubeConnections(GridPos gridPos, AmidaTube.DirectionPassage checkDirectionPassage)
    {
        // �e�����̃O���b�h�ʒu���v�Z
        GridPos[] positions = {
            new GridPos(gridPos.x, gridPos.y - 1), // up
            new GridPos(gridPos.x, gridPos.y + 1), // down
            new GridPos(gridPos.x + 1, gridPos.y), // right
            new GridPos(gridPos.x - 1, gridPos.y)  // left
        };

        // �e������AmidaTube���擾
        AmidaTube[] amidaTubes = new AmidaTube[4];
        for (int i = 0; i < positions.Length; i++)
        {
            if (m_map.CheckInnerGridPos(positions[i]))
            {
                amidaTubes[i] = m_amidaTubeGrid[positions[i].y, positions[i].x]?.GetComponent<AmidaTube>();
            }
        }

        // �ʉߕ�����ݒ�
        EnsureTubeConnection(checkDirectionPassage, amidaTubes[0], Direction.UP);
        EnsureTubeConnection(checkDirectionPassage, amidaTubes[1], Direction.DOWN);
        EnsureTubeConnection(checkDirectionPassage, amidaTubes[2], Direction.RIGHT);
        EnsureTubeConnection(checkDirectionPassage, amidaTubes[3], Direction.LEFT);
    }

    /// <summary>
    /// �w�肵�������Ɉړ��ł��邩�ǂ���
    /// </summary>
    /// <param name="gridPos">���݂̃O���b�h�ʒu</param>
    /// <param name="direction">�ړ�����</param>
    /// <returns>�ړ��ł���ꍇ�� true�A����ȊO�̏ꍇ�� false</returns>
    private bool CanMovementDirection(GridPos gridPos, Direction direction)
    {
        if (CanMove(gridPos) == false) return false;

        AmidaTube currentAmida = m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>();
        AmidaTube.DirectionPassage passage = currentAmida.GetDirectionPassage();

        switch(direction)
        {
            case Direction.UP:
                if (!passage.down) return false;
                break;
            case Direction.DOWN:
                if (!passage.up) return false;
                break;
            case Direction.RIGHT:
                if (!passage.left) return false;
                break;
            case Direction.LEFT:
                if (!passage.right) return false;
                break;

        }

        return true;
    }

    /// <summary>
    /// �ʉߕ����̃`���[�u�ƌq�����Ă��Ȃ��ꍇ�V�����`���[�u��ݒ肷��֐�
    /// </summary>
    /// <param name="checkDirectionPassage">�ʉߕ���</param>
    /// <param name="neighborTube">�אڂ��邠�݂��`���[�u</param>
    /// <param name="checkDirection">���ׂ����</param>
    private void EnsureTubeConnection(AmidaTube.DirectionPassage checkDirectionPassage, AmidaTube neighborTube, Direction checkDirection)
    {
        if (neighborTube != null)
        {
            AmidaTube.DirectionPassage directionPassage = neighborTube.GetDirectionPassage();
            switch (checkDirection)
            {
                case Direction.UP:
                    if (checkDirectionPassage.up && !directionPassage.down)
                        directionPassage.down = true;
                    else
                        directionPassage.down = false;
                    neighborTube.SetDirectionPassage(directionPassage);
                    break;
                case Direction.DOWN:
                    if (checkDirectionPassage.down && !directionPassage.up)
                        directionPassage.up = true;
                    else
                        directionPassage.up = false;
                    neighborTube.SetDirectionPassage(directionPassage);
                    break;
                case Direction.RIGHT:
                    if (checkDirectionPassage.right && !directionPassage.left)
                        directionPassage.left = true;
                    else
                        directionPassage.left = false;
                    neighborTube.SetDirectionPassage(directionPassage);
                    break;
                case Direction.LEFT:
                    if (checkDirectionPassage.left && !directionPassage.right)
                        directionPassage.right = true;
                    else
                        directionPassage.right = false;
                    neighborTube.SetDirectionPassage(directionPassage);
                    break;
            }
        }
    }

    /// <summary>
    /// ���݂��̕�����I�����鏈��
    /// </summary>
    /// <param name="gridPos">�O���b�h�ʒu</param>
    /// <param name="movementDirection">�ړ�����</param>
    /// <param name="electricFlowType">�d�C�̎��</param>
    /// <returns>�ړ��ł���ꍇ�� true�A����ȊO�̏ꍇ�� false</returns>
    private bool SelectAmidaDirection(GridPos gridPos, Direction movementDirection, Electric.ElectricFlowType electricFlowType)
    {
        // �������Ă��Ȃ��ꍇ�͏������΂�
        if (m_amidaTubeGrid[gridPos.y, gridPos.x] == null || !m_amidaTubeGrid[gridPos.y, gridPos.x].activeSelf) return false;

        AmidaTube.DirectionPassage passage = m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage();

        // �������Ɉړ�
        if (movementDirection == Direction.RIGHT || movementDirection == Direction.LEFT)
        {
            if (passage.up)
            {
                Move(gridPos, Direction.UP, electricFlowType);
                return true;
            }
            if (passage.down)
            {
                Move(gridPos, Direction.DOWN, electricFlowType);
                return true;
            }

            // ���������ɐi�ݑ�����
            Move(gridPos, movementDirection, electricFlowType);
            return true;
        }

        // �c�����Ɉړ�
        if (movementDirection == Direction.UP || movementDirection == Direction.DOWN)
        {
            if (passage.right)
            {
                Move(gridPos, Direction.RIGHT, electricFlowType);
                return true;
            }

            // ���������ɐi�ݑ�����
            Move(gridPos, movementDirection, electricFlowType);
            return true;
        }

        return false;
    }

    /// <summary>
    /// �w������Ɉړ������鏈��
    /// </summary>
    /// <param name="gridPos">�O���b�h�ʒu</param>
    /// <param name="movementDirection">�ړ�����</param>
    /// <param name="electricFlowType">�d�C�̎��</param>
    private void Move(GridPos gridPos, Direction movementDirection, Electric.ElectricFlowType electricFlowType)
    {
        // �������Ă��Ȃ��ꍇ�͏������΂�
        if (m_amidaTubeGrid[gridPos.y, gridPos.x] == null || !m_amidaTubeGrid[gridPos.y, gridPos.x].activeSelf) return;

        AmidaTube.DirectionPassage passage = m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage();

        // �ړ����Ă���I�u�W�F�N�g
        GameObject movingObject = m_amidaTubeGrid[gridPos.y, gridPos.x];

        // ���O�Ɉړ�������ۑ�
        Direction prevMovementDirection = movementDirection;

        bool endMovement = false;

        // x�����Ɉړ�
        if (movementDirection == Direction.RIGHT)
        {
            if (gridPos.x + 1 < m_map.GetCommonData().width)
                gridPos.x = gridPos.x + 1;
            else
                endMovement = true;

            movementDirection = Direction.RIGHT;
        }
        // y�����Ɉړ�
        else if (movementDirection == Direction.DOWN)
        {
            if (gridPos.y + 1 < m_map.GetCommonData().height)
                gridPos.y = gridPos.y + 1;
            else
                endMovement = true;

            movementDirection = Direction.DOWN;
        }
        // y�����Ɉړ�
        else if (movementDirection == Direction.UP)
        {
            if (gridPos.y - 1 >= 0)
                gridPos.y = gridPos.y - 1;
            else
                endMovement = true;

            movementDirection = Direction.UP;
        }
        else
            endMovement = true;

        if (movingObject)
        // �ړ����Ă����I�u�W�F�N�g�ɓd�C�𗬂�
        ConductElectricity(GetElectricMaterial(electricFlowType).color, movingObject, prevMovementDirection, electricFlowType);

        // �I������ꍇ
        if (endMovement) return;

        // �i�񂾐���m�F����
        if (CanMove(gridPos) == false) return;

        // �ړ��I�u�W�F�N�g�̍X�V
        movingObject = m_amidaTubeGrid[gridPos.y, gridPos.x].gameObject;

        // ���Ε����ɂ���
        Direction counterDirection = movementDirection;
        switch (movementDirection)
        {
            case Direction.UP: counterDirection = Direction.DOWN; break;
            case Direction.DOWN: counterDirection = Direction.UP; break;
            case Direction.LEFT: counterDirection = Direction.RIGHT; break;
            case Direction.RIGHT: counterDirection = Direction.LEFT; break;
        }

        // �V�����ړ��I�u�W�F�N�g�̐F��ύX����
        ConductElectricity(GetElectricMaterial(electricFlowType).color, movingObject, counterDirection, electricFlowType);

        // ���݂��̕��������߂�
        SelectAmidaDirection(gridPos, movementDirection, electricFlowType);
    }

    /// <summary>
    /// ���݂��̐F��ύX���鏈��
    /// </summary>
    /// <param name="color">�ύX�F</param>
    /// <param name="amidaObject">�ύX���邠�݂��I�u�W�F�N�g</param>
    /// <param name="direction">�ړ�����</param>
    /// <param name="electricFlowType">�d�C�̎��</param>
    private void ConductElectricity(Color32 color, GameObject amidaObject, Direction direction, Electric.ElectricFlowType electricFlowType)
    {
        AmidaTube amidaTube = amidaObject.GetComponent<AmidaTube>();
        //amidaTube.ConductElectricity(color, direction, electricFlowType);
        amidaTube.ConductElectricity(m_baseTexture);
    }
    /// <summary>
    /// ���݂��������J�n���鏈��
    /// </summary>
    /// <param name="gridPos">�O���b�h�ʒu</param>
    /// <param name="electricFlowType">�d�C�̎��</param>
    /// <param name="startDirection">�J�n����</param>
    private void StartAmidakuji(GridPos gridPos, Electric.ElectricFlowType electricFlowType, Direction startDirection)
    {
        // �ړ��\���ǂ������m�F
        if (CanMovementDirection(gridPos, startDirection) == false) return;

        // �����̈ړ�������ݒ�
        Direction movementDirection = Direction.RIGHT;

        // �ʉߕ����Ɋ�Â��Ĉړ�����������
        if (m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage().right)
            movementDirection = Direction.RIGHT;
        if (m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage().down)
            movementDirection = Direction.DOWN;
        if (m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage().up)
            movementDirection = Direction.UP;

        // ���Ε�����ݒ�
        Direction counterDirection = startDirection;
        switch (movementDirection)
        {
            case Direction.UP: counterDirection = Direction.DOWN; break;
            case Direction.DOWN: counterDirection = Direction.UP; break;
            case Direction.LEFT: counterDirection = Direction.RIGHT; break;
            case Direction.RIGHT: counterDirection = Direction.LEFT; break;
        }

        // ���݂��I�u�W�F�N�g�̐F��ύX
        ConductElectricity(GetElectricMaterial(electricFlowType).color, m_amidaTubeGrid[gridPos.y, gridPos.x].gameObject, Direction.LEFT, electricFlowType);

        // �E�����Ɉړ�
        Move(gridPos, movementDirection, electricFlowType);
    }

    /// <summary>
    /// �S�Ẵu���b�N�̐F�����Z�b�g����
    /// </summary>
    private void ResetAllBlockColor()
    {
        for (int cy = 0; cy < m_map.GetCommonData().height; cy++)
        {
            for (int cx = 0; cx < m_map.GetCommonData().width; cx++)
            {
                if (m_amidaTubeGrid[cy, cx] && m_amidaTubeGrid[cy, cx].CompareTag("AmidaTube"))
                    m_amidaTubeGrid[cy, cx].GetComponent<AmidaTube>().ResetState();
            }
        }
    }

    /// <summary>
    /// �ړ��ł��邩�ǂ������m�F����
    /// </summary>
    /// <param name="gridPos">�ړ�����\��̃O���b�h�ʒu</param>
    /// <returns>�ړ��ł���ꍇ�� true�A����ȊO�̏ꍇ�� false</returns>
    private bool CanMove(GridPos gridPos)
    {
        if (m_map.CheckInnerGridPos(gridPos) == false) return false;
        if (m_amidaTubeGrid[gridPos.y, gridPos.x] == null || !m_amidaTubeGrid[gridPos.y, gridPos.x].activeSelf) return false;
        if (m_amidaTubeGrid[gridPos.y, gridPos.x].tag != "AmidaTube") return false;

        return true;
    }

    /// <summary>
    /// �d�C�̎�ނɉ������}�e���A�����擾����
    /// </summary>
    /// <param name="electricFlowType">�d�C�̎��</param>
    /// <returns>�Ή�����}�e���A��</returns>
    private Material GetElectricMaterial(Electric.ElectricFlowType electricFlowType)
    {
        switch (electricFlowType)
        {
            case Electric.ElectricFlowType.NORMAL:
                return m_amidaElectricityMaterial;
            case Electric.ElectricFlowType.RED:
                return m_amidaRedElectricityMaterial;
        }

        return m_amidaElectricityMaterial;
    }

    /// <summary>
    /// ���݂������̊J�n�v����ǉ�����
    /// </summary>
    /// <param name="gridPos">�O���b�h�ʒu</param>
    /// <param name="electricFlowType">�d�C�̎��</param>
    /// <param name="startDirection">�J�n����</param>
    public void RequestStartAmida(GridPos gridPos, Electric.ElectricFlowType electricFlowType, Direction startDirection)
    {
        AmidaMoveCash amidaMoveCash;
        amidaMoveCash.gridPos = gridPos;
        amidaMoveCash.electricFlowType = electricFlowType;
        amidaMoveCash.movementDir = startDirection;

        m_amidaMoveCash.Add(amidaMoveCash);
    }
}
