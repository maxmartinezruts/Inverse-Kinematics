using System;
namespace MatrixInverse
{
  class MatrixInverseProgram
  {
    static void Main(string[] args)
    {
      Console.WriteLine("\nBegin matrix inverse using Crout LU decomp demo \n");

      float[][] m = MatrixCreate(4, 4); 
      m[0][0] = 3.0f; m[0][1] = 7.0f; m[0][2] = 2.0f; m[0][3] = 5.0f;
      m[1][0] = 1.0f; m[1][1] = 8.0f; m[1][2] = 4.0f; m[1][3] = 2.0f;
      m[2][0] = 2.0f; m[2][1] = 1.0f; m[2][2] = 9.0f; m[2][3] = 3.0f;
      m[3][0] = 5.0f; m[3][1] = 4.0f; m[3][2] = 7.0f; m[3][3] = 1.0f;


      Console.WriteLine("Original matrix m is ");
      Console.WriteLine(MatrixAsString(m));

      float d = MatrixDeterminant(m);
      if (Math.Abs(d) < 1.0e-5)
        Console.WriteLine("Matrix has no inverse");

      float[][] inv = MatrixInverse(m);

      Console.WriteLine("Inverse matrix inv is ");
      Console.WriteLine(MatrixAsString(inv));

      float[][] prod = MatrixProduct(m, inv);
      Console.WriteLine("The product of m * inv is ");
      Console.WriteLine(MatrixAsString(prod));

      Console.WriteLine("========== \n");

      float[][] lum;
      int[] perm;
      int toggle = MatrixDecompose(m, out lum, out perm);
      Console.WriteLine("The combined lower-upper decomposition of m is");
      Console.WriteLine(MatrixAsString(lum));

      float[][] lower = ExtractLower(lum);
      float[][] upper = ExtractUpper(lum);

      Console.WriteLine("The lower part of LUM is");
      Console.WriteLine(MatrixAsString(lower));

      Console.WriteLine("The upper part of LUM is");
      Console.WriteLine(MatrixAsString(upper));

      Console.WriteLine("The perm[] array is");
      ShowVector(perm);

      float[][] lowTimesUp = MatrixProduct(lower, upper);
      Console.WriteLine("The product of lower * upper is ");
      Console.WriteLine(MatrixAsString(lowTimesUp));


      Console.WriteLine("\nEnd matrix inverse demo \n");
      Console.ReadLine();

    } // Main

    static void ShowVector(int[] vector)
    {
      Console.Write("   ");
      for (int i = 0; i < vector.Length; ++i)
        Console.Write(vector[i] + " ");
      Console.WriteLine("\n");
    }

    public static float[][] MatrixInverse(float[][] matrix)
    {
      // assumes determinant is not 0
      // that is, the matrix does have an inverse
      int n = matrix.Length;
      float[][] result = MatrixCreate(n, n); // make a copy of matrix
      for (int i = 0; i < n; ++i)
        for (int j = 0; j < n; ++j)
          result[i][j] = matrix[i][j];

      float[][] lum; // combined lower & upper
      int[] perm;
      int toggle;
      toggle = MatrixDecompose(matrix, out lum, out perm);

      float[] b = new float[n];
      for (int i = 0; i < n; ++i)
      {
        for (int j = 0; j < n; ++j)
          if (i == perm[j])
            b[j] = 1.0f;
          else
            b[j] = 0.0f;
 
        float[] x = Helper(lum, b); // 
        for (int j = 0; j < n; ++j)
          result[j][i] = x[j];
      }
      return result;
    } // MatrixInverse

    static int MatrixDecompose(float[][] m, out float[][] lum, out int[] perm)
    {
      // Crout's LU decomposition for matrix determinant and inverse
      // stores combined lower & upper in lum[][]
      // stores row permuations into perm[]
      // returns +1 or -1 according to even or odd number of row permutations
      // lower gets dummy 1.0s on diagonal (0.0s above)
      // upper gets lum values on diagonal (0.0s below)

      int toggle = +1; // even (+1) or odd (-1) row permutatuions
      int n = m.Length;

      // make a copy of m[][] into result lu[][]
      lum = MatrixCreate(n, n);
      for (int i = 0; i < n; ++i)
        for (int j = 0; j < n; ++j)
          lum[i][j] = m[i][j];


      // make perm[]
      perm = new int[n];
      for (int i = 0; i < n; ++i)
        perm[i] = i;

      for (int j = 0; j < n - 1; ++j) // process by column. note n-1 
      {
        float max = Math.Abs(lum[j][j]);
        int piv = j;

        for (int i = j + 1; i < n; ++i) // find pivot index
        {
          float xij = Math.Abs(lum[i][j]);
          if (xij > max)
          {
            max = xij;
            piv = i;
          }
        } // i

        if (piv != j)
        {
          float[] tmp = lum[piv]; // swap rows j, piv
          lum[piv] = lum[j];
          lum[j] = tmp;

          int t = perm[piv]; // swap perm elements
          perm[piv] = perm[j];
          perm[j] = t;

          toggle = -toggle;
        }

        float xjj = lum[j][j];
        if (xjj != 0.0)
        {
          for (int i = j + 1; i < n; ++i)
          {
            float xij = lum[i][j] / xjj;
            lum[i][j] = xij;
            for (int k = j + 1; k < n; ++k)
              lum[i][k] -= xij * lum[j][k];
          }
        }

      } // j

      return toggle;
    } // MatrixDecompose

    static float[] Helper(float[][] luMatrix, float[] b) // helper
    {
      int n = luMatrix.Length;
      float[] x = new float[n];
      b.CopyTo(x, 0);

      for (int i = 1; i < n; ++i)
      {
        float sum = x[i];
        for (int j = 0; j < i; ++j)
          sum -= luMatrix[i][j] * x[j];
        x[i] = sum;
      }

      x[n - 1] /= luMatrix[n - 1][n - 1];
      for (int i = n - 2; i >= 0; --i)
      {
        float sum = x[i];
        for (int j = i + 1; j < n; ++j)
          sum -= luMatrix[i][j] * x[j];
        x[i] = sum / luMatrix[i][i];
      }

      return x;
    } // Helper

    public static float MatrixDeterminant(float[][] matrix)
    {
      float[][] lum;
      int[] perm;
      int toggle = MatrixDecompose(matrix, out lum, out perm);
      float result = toggle;
      for (int i = 0; i < lum.Length; ++i)
        result *= lum[i][i];
      return result;
    }

    public static float [][] Identity(int dim)
    {
      float[][] result = MatrixCreate(dim,dim);
 
      for (int i = 0; i < dim; ++i){
        result[i][i] = 1f;

      }
      return result;
    }

    public static float[] MatrixToEuler(float[][] matrix){

        float[] result = new float[3];
        result[0] = (float)(Math.Atan2(matrix[1][2], matrix[0][2]));
        result[1] = (float)(Math.Atan2(Math.Sqrt(Math.Pow(matrix[0][2],2)- Math.Pow(matrix[1][2],2)),matrix[2][2]));
        result[2] = (float)(Math.Atan2(matrix[2][1], -matrix[2][0]));

        return result;
    }

    public static float Trace(float[][] matrix){

        float result = 0;
        for (int i = 0; i < matrix.Length; ++i){
            result += matrix[i][i];
        }
      
        return result;
    }
    public static float[] MatToQuat(float[][] C){

        float[] result = new float[4];
        result[0] = 0.5f*(float)(Math.Sqrt(1+Trace(C)));
        result[1] = 0.5f*(float)(Math.Sign(C[2][1]-C[1][2])*Math.Sqrt(Math.Max(C[0][0]-C[1][1]-C[2][2] + 1f,0)));
        result[2] = 0.5f*(float)(Math.Sign(C[0][2]-C[2][0])*Math.Sqrt(Math.Max(C[1][1]-C[2][2]-C[0][0] + 1f,0f)));
        result[3] = 0.5f*(float)(Math.Sign(C[1][0]-C[0][1])*Math.Sqrt(Math.Max(C[2][2]-C[0][0]-C[1][1] + 1f,0)));
        return result;
    }

    // ----------------------------------------------------------------

    public static float[][] MatrixCreate(int rows, int cols)
    {
      float[][] result = new float[rows][];
      for (int i = 0; i < rows; ++i)
        result[i] = new float[cols];
      return result;
    }

    public static float[][] MatrixProduct(float[][] matrixA,
      float[][] matrixB)
    {
      int aRows = matrixA.Length;
      int aCols = matrixA[0].Length;
      int bRows = matrixB.Length;
      int bCols = matrixB[0].Length;
      if (aCols != bRows)
        throw new Exception("Non-conformable matrices");

      float[][] result = MatrixCreate(aRows, bCols);

      for (int i = 0; i < aRows; ++i) // each row of A
        for (int j = 0; j < bCols; ++j) // each col of B
          for (int k = 0; k < aCols; ++k) // could use k < bRows
            result[i][j] += matrixA[i][k] * matrixB[k][j];

      return result;
    }

    static string MatrixAsString(float[][] matrix)
    {
      string s = "";
      for (int i = 0; i < matrix.Length; ++i)
      {
        for (int j = 0; j < matrix[i].Length; ++j)
          s += matrix[i][j].ToString("F3").PadLeft(8) + " ";
        // s += Environment.NewLine;
      }
      return s;
    }

    static float[][] ExtractLower(float[][] lum)
    {
      // lower part of an LU Doolittle decomposition (dummy 1.0s on diagonal, 0.0s above)
      int n = lum.Length;
      float[][] result = MatrixCreate(n, n);
      for (int i = 0; i < n; ++i)
      {
        for (int j = 0; j < n; ++j)
        {
          if (i == j)
            result[i][j] = 1.0f;
          else if (i > j)
            result[i][j] = lum[i][j];
        }
      }
      return result;
    }

    static float[][] ExtractUpper(float[][] lum)
    {
      // upper part of an LU (lu values on diagional and above, 0.0s below)
      int n = lum.Length;
      float[][] result = MatrixCreate(n, n);
      for (int i = 0; i < n; ++i)
      {
        for (int j = 0; j < n; ++j)
        {
          if (i <= j)
            result[i][j] = lum[i][j];
        }
      }
      return result;
    }
    public static float[][] CrossProduct(float[][] v1, float[][] v2)
  {
    float x, y, z;

    float [][] A = new float[][]{new float[]{0, -v1[2][0], v1[1][0]}, new float[]{v1[2][0], 0, -v1[0][0]}, new float[]{-v1[1][0],v1[0][0],0}};
    float [][] result = MatrixProduct(A,v2);


    return result;
  }

  public static float[][] MatrixSubstract(float[][] M1, float[][] M2){
    int n = M1.Length;
    int m = M1[0].Length;
    float[][] result = MatrixCreate(n,m);
      for (int i = 0; i < n; ++i)
        for (int j = 0; j < m; ++j)
          result[i][j] = M1[i][j] - M2[i][j];
    return result;
  }

  public static float[][] MatrixSum(float[][] M1, float[][] M2){
    int n = M1.Length;
    int m = M1[0].Length;
    float[][] result = MatrixCreate(n,m);
      for (int i = 0; i < n; ++i)
        for (int j = 0; j < m; ++j)
          result[i][j] = M1[i][j] + M2[i][j];
    return result;
  }

  public static float[][] MatrixMultiply(float[][] M, float P){
    int n = M.Length;
    int m = M[0].Length;
    float[][] result = MatrixCreate(n,m);
      for (int i = 0; i < n; ++i)
        for (int j = 0; j < m; ++j)
          result[i][j] = M[i][j] *P;
    return result;
  }

  public static float[][] TransformToPosition(float[][] T){
    float[][] result = new float[3][];
    result[0] = new float[1]{T[0][3]};
    result[1] = new float[1]{T[1][3]};
    result[2] = new float[1]{T[2][3]};
    return result;
  }

  public static float[][] TransformToRotation(float[][] T){
    float[][] result = new float[3][];
    result[0] = new float[3]{T[0][0],T[0][1],T[0][2]};
    result[1] = new float[3]{T[1][0],T[1][1],T[1][2]};
    result[2] = new float[3]{T[2][0],T[2][1],T[2][2]};
    return result;
  }

  public static float[][] Transpose(float[][] A){
      float[][] result = MatrixCreate(A[0].Length,A.Length);
      for (int i = 0; i < A.Length; i++)
      {
        for (int j = 0; j < A[0].Length; j++)
        {
            result[j][i] = A[i][j];
        }
      }
      return result;
}

public static float[][] ProductScalarMatrix(float scalar, float[][] A){
  for (int i = 0; i < A.Length; i++)
  {
      for (int j = 0; j < A[0].Length; j++)
      {
          A[i][j] *= scalar;
      }
  }
  return A;
}

  public static float[][] PseudoInverseDamped(float [][] A, float lambda){
    return MatrixProduct(Transpose(A),MatrixInverse(MatrixSum(MatrixProduct(A,Transpose(A)),Identity(A.Length))));
  }
    // ----------------------------------------------------------------

  } // Program 

} // ns
