using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBox1.Text == "")
        {
            Response.Write("<script>window.alert('没有输入用户名');</script>");
            return;
        }
          
        else if(TextBox2.Text == "")
        {
            Response.Write("<script>window.alert('没有输入密码');</script>");
            return;
        }
             
        SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=login_webhw;Integrated Security=True");
        // 这句话从sqldatasource控件里面复制一个出来。
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT *FROM logindb where userID = '" + TextBox1.Text.Trim() + "' and userPWD = '" + TextBox2.Text.Trim() + "'",conn);
        // 这句话创建一个指令
        SqlDataReader sdr = cmd.ExecuteReader();//指令传给reader
        Label1.Text=cmd.CommandText.ToString();//查看一下自己的查询语句对不对，一会注释掉
        // 这句执行它
        sdr.Read();
        if (sdr.HasRows)
            Response.Write("<script>window.alert('登录成功');</script>");
        else//最讨厌的就是一个网页告诉我用户名或者密码错误，但是不告诉我到底是哪个
        {
            sdr.Close();//这里本来不知道要写的，但是下面再次调用出现了小问题，就加上。最好用完就关掉
            cmd.CommandText = "SELECT *FROM logindb where userID = '" + TextBox1.Text.Trim() +"'";
            sdr = cmd.ExecuteReader();//指令传给reader
            if (sdr.HasRows)
                Response.Write("<script>window.alert('密码错误');</script>");
            else
                 Response.Write("<script>window.alert('用户名不存在');</script>");
            sdr.Close();
        }
           
        conn.Close();
    }


    protected void Button2_Click(object sender, EventArgs e)//注册按钮
    {
        SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=login_webhw;Integrated Security=True");
        conn.Open();
        //先检查有没有已经存在了，这个用户
        SqlCommand cmd = new SqlCommand("SELECT *FROM logindb where userID = '" + TextBox1.Text.Trim() + "'", conn);
        SqlDataReader sdr = cmd.ExecuteReader();
        Label1.Text = cmd.CommandText.ToString();//查看一下自己的查询语句对不对，一会注释掉
        sdr.Read();
        if (sdr.HasRows)
            Response.Write("<script>window.alert('用户名已经存在，不可以重复注册');</script>");
        else//开始写注册的东东
        {
            sdr.Close();
            cmd.CommandText = "insert into logindb (userID,userPWD) values ('"+TextBox1.Text+"','"+TextBox2.Text+"')";
            string rows_effedted = cmd.ExecuteNonQuery().ToString();
            Label1.Text = rows_effedted;
            conn.Close();
            conn.Dispose();//释放conn所有的资源。
            Response.Write("<script>window.alert('注册成功，可以登录了');</script>");
        }

    }
}