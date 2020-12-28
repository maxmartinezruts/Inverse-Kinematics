using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour

{

    public GameObject[] segments;

    public Transform reference;
    public float [] q_1d;

    public float [] r_des;

    public float [][] q = new float[5][];

    public float [] l;

    // Start is called before the first frame update

    // float [][] jointToRotMat(float[][] joint){

    // }
    void Start()
    {
        print("hello world");
        print(l.Length);
        print(l[2]);

        q[0] = new float[1]{q_1d[0]};
        q[1] = new float[1]{q_1d[1]};
        q[2] = new float[1]{q_1d[2]};
        q[3] = new float[1]{q_1d[3]};
        q[4] = new float[1]{q_1d[4]};



    }

    // Update is called once per frame
    void Update()
    {
        
        float [][] TI0 = MatrixInverse.MatrixInverseProgram.Identity(4);
        float [][] T01 = new float[4][];
        T01[0] = new float[4]{Mathf.Cos(q[0][0]),-Mathf.Sin(q[0][0]),0,0};
        T01[1] = new float[4]{Mathf.Sin(q[0][0]),Mathf.Cos(q[0][0]),0,0};
        T01[2] = new float[4]{0,0,1,l[0]};
        T01[3] = new float[4]{0,0,0,1};

        float [][] T12 = new float[4][];
 
        T12[0] = new float[4]{1,0,0,0};
        T12[1] = new float[4]{0,Mathf.Cos(q[1][0]),-Mathf.Sin(q[1][0]),0};
        T12[2] = new float[4]{0,Mathf.Sin(q[1][0]),Mathf.Cos(q[1][0]),l[1]};
        T12[3] = new float[4]{0,0,0,1};

        float [][] T23 = new float[4][];
        T23[0] = new float[4]{1,0,0,0};
        T23[1] = new float[4]{0,Mathf.Cos(q[2][0]),-Mathf.Sin(q[2][0]),0};
        T23[2] = new float[4]{0,Mathf.Sin(q[2][0]),Mathf.Cos(q[2][0]),l[2]};
        T23[3] = new float[4]{0,0,0,1};

        float [][] T34 = new float[4][];
        T34[0] = new float[4]{Mathf.Cos(q[3][0]),0,Mathf.Sin(q[3][0]),0};
        T34[1] = new float[4]{0,1,0,1.34f};
        T34[2] = new float[4]{-Mathf.Sin(q[3][0]),0,Mathf.Cos(q[3][0]),0.7f};
        T34[3] = new float[4]{0,0,0,1};

        float [][] T45 = new float[4][];
        T45[0] = new float[4]{1,0,0,0};
        T45[1] = new float[4]{0,Mathf.Cos(q[4][0]),-Mathf.Sin(q[4][0]),1.68f};
        T45[2] = new float[4]{0,Mathf.Sin(q[4][0]),Mathf.Cos(q[4][0]),0};
        T45[3] = new float[4]{0,0,0,1};

        float [][] T5E = new float[4][];
        T5E[0] = new float[4]{1,0,0,0};
        T5E[1] = new float[4]{0,1,0,0.72f};
        T5E[2] = new float[4]{0,0,1,0};
        T5E[3] = new float[4]{0,0,0,1};

        float[][] TI1 = MatrixInverse.MatrixInverseProgram.MatrixProduct(TI0,T01);
        float[][] TI2 = MatrixInverse.MatrixInverseProgram.MatrixProduct(TI1,T12);
        float[][] TI3 = MatrixInverse.MatrixInverseProgram.MatrixProduct(TI2,T23);
        float[][] TI4 = MatrixInverse.MatrixInverseProgram.MatrixProduct(TI3,T34);
        float[][] TI5 = MatrixInverse.MatrixInverseProgram.MatrixProduct(TI4,T45);
        float[][] TIE = MatrixInverse.MatrixInverseProgram.MatrixProduct(TI5,T5E);

        float [][] FI_rI0 = new float [4][];
        FI_rI0[0] = new float[1]{0};
        FI_rI0[1] = new float[1]{0};
        FI_rI0[2] = new float[1]{0};
        FI_rI0[3] = new float[1]{0};

        float [][] I_rI0 = MatrixInverse.MatrixInverseProgram.TransformToPosition(TI0);
        float [][] I_rI1 = MatrixInverse.MatrixInverseProgram.TransformToPosition(TI1);
        float [][] I_rI2 = MatrixInverse.MatrixInverseProgram.TransformToPosition(TI2);
        float [][] I_rI3 = MatrixInverse.MatrixInverseProgram.TransformToPosition(TI3);
        float [][] I_rI4 = MatrixInverse.MatrixInverseProgram.TransformToPosition(TI4);
        float [][] I_rI5 = MatrixInverse.MatrixInverseProgram.TransformToPosition(TI5);
        float [][] I_rIE = MatrixInverse.MatrixInverseProgram.TransformToPosition(TIE);


        float [][] RI0 = MatrixInverse.MatrixInverseProgram.TransformToRotation(TI0);
        float [][] RI1 = MatrixInverse.MatrixInverseProgram.TransformToRotation(TI1);
        float [][] RI2 = MatrixInverse.MatrixInverseProgram.TransformToRotation(TI2);
        float [][] RI3 = MatrixInverse.MatrixInverseProgram.TransformToRotation(TI3);
        float [][] RI4 = MatrixInverse.MatrixInverseProgram.TransformToRotation(TI4);
        float [][] RI5 = MatrixInverse.MatrixInverseProgram.TransformToRotation(TI5);
        float [][] RIE = MatrixInverse.MatrixInverseProgram.TransformToRotation(TIE);



        float [] QI0 = MatrixInverse.MatrixInverseProgram.MatToQuat(RI0);
        float [] QI1 = MatrixInverse.MatrixInverseProgram.MatToQuat(RI1);
        float [] QI2 = MatrixInverse.MatrixInverseProgram.MatToQuat(RI2);
        float [] QI3 = MatrixInverse.MatrixInverseProgram.MatToQuat(RI3);
        float [] QI4 = MatrixInverse.MatrixInverseProgram.MatToQuat(RI4);
        float [] QI5 = MatrixInverse.MatrixInverseProgram.MatToQuat(RI5);
        float [] QIE = MatrixInverse.MatrixInverseProgram.MatToQuat(RIE);




        float [][] n_1 = new float[3][];
        n_1[0] = new float[1]{0};
        n_1[1] = new float[1]{0};
        n_1[2] = new float[1]{1};

        float [][] n_2 = new float[3][];
        n_2[0] = new float[1]{1};
        n_2[1] = new float[1]{0};
        n_2[2] = new float[1]{0};

        float [][] n_3 = new float[3][];
        n_3[0] = new float[1]{1};
        n_3[1] = new float[1]{0};
        n_3[2] = new float[1]{0};

        float [][] n_4 = new float[3][];
        n_4[0] = new float[1]{0};
        n_4[1] = new float[1]{1};
        n_4[2] = new float[1]{0};

        float [][] n_5 = new float[3][];
        n_5[0] = new float[1]{1};
        n_5[1] = new float[1]{0};
        n_5[2] = new float[1]{0};

        float [][] Jp = new float[3][];
        float[][] col1 = MatrixInverse.MatrixInverseProgram.CrossProduct(MatrixInverse.MatrixInverseProgram.MatrixProduct(RI1,n_1), MatrixInverse.MatrixInverseProgram.MatrixSubstract(I_rIE,I_rI1));
        float[][] col2 = MatrixInverse.MatrixInverseProgram.CrossProduct(MatrixInverse.MatrixInverseProgram.MatrixProduct(RI2,n_2), MatrixInverse.MatrixInverseProgram.MatrixSubstract(I_rIE,I_rI2));
        float[][] col3 = MatrixInverse.MatrixInverseProgram.CrossProduct(MatrixInverse.MatrixInverseProgram.MatrixProduct(RI3,n_3), MatrixInverse.MatrixInverseProgram.MatrixSubstract(I_rIE,I_rI3));
        float[][] col4 = MatrixInverse.MatrixInverseProgram.CrossProduct(MatrixInverse.MatrixInverseProgram.MatrixProduct(RI4,n_4), MatrixInverse.MatrixInverseProgram.MatrixSubstract(I_rIE,I_rI4));
        float[][] col5 = MatrixInverse.MatrixInverseProgram.CrossProduct(MatrixInverse.MatrixInverseProgram.MatrixProduct(RI5,n_5), MatrixInverse.MatrixInverseProgram.MatrixSubstract(I_rIE,I_rI5));

        Jp[0] = new float[5]{col1[0][0],col2[0][0],col3[0][0],col4[0][0],col5[0][0]};
        Jp[1] = new float[5]{col1[1][0],col2[1][0],col3[1][0],col4[1][0],col5[1][0]};
        Jp[2] = new float[5]{col1[2][0],col2[2][0],col3[2][0],col4[2][0],col5[2][0]};

        float [][] Jpinv = MatrixInverse.MatrixInverseProgram.PseudoInverseDamped(Jp,0.01f);

        float [][] I_r_IE_des = new float[3][];
        I_r_IE_des[0] = new float[1]{reference.transform.position.x};
        I_r_IE_des[1] = new float[1]{reference.transform.position.z};
        I_r_IE_des[2] = new float[1]{reference.transform.position.y};

        float[][] dr = MatrixInverse.MatrixInverseProgram.MatrixSubstract(I_r_IE_des, I_rIE);
        q = MatrixInverse.MatrixInverseProgram.MatrixSum(q, MatrixInverse.MatrixInverseProgram.ProductScalarMatrix(0.15f,MatrixInverse.MatrixInverseProgram.MatrixProduct(Jpinv, dr)));
        

        segments[0].transform.position = new Vector3(I_rI0[0][0], I_rI0[2][0], I_rI0[1][0]);
        segments[1].transform.position = new Vector3(I_rI1[0][0], I_rI1[2][0], I_rI1[1][0]);
        segments[2].transform.position = new Vector3(I_rI2[0][0], I_rI2[2][0], I_rI2[1][0]);
        segments[3].transform.position = new Vector3(I_rI3[0][0], I_rI3[2][0], I_rI3[1][0]);
        segments[4].transform.position = new Vector3(I_rI4[0][0], I_rI4[2][0], I_rI4[1][0]);
        segments[5].transform.position = new Vector3(I_rI5[0][0], I_rI5[2][0], I_rI5[1][0]);


        segments[0].transform.rotation = new Quaternion( -QI0[1], -QI0[3], -QI0[2], QI0[0]);
        segments[1].transform.rotation = new Quaternion( -QI1[1], -QI1[3], -QI1[2], QI1[0]);
        segments[2].transform.rotation = new Quaternion( -QI2[1], -QI2[3], -QI2[2], QI2[0]);
        segments[3].transform.rotation = new Quaternion( -QI3[1], -QI3[3], -QI3[2], QI3[0]);
        segments[4].transform.rotation = new Quaternion( -QI4[1], -QI4[3], -QI4[2], QI4[0]);
        segments[5].transform.rotation = new Quaternion( -QI5[1], -QI5[3], -QI5[2], QI5[0]);




        
    }
}
