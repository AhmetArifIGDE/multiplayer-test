using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class oyuncu : MonoBehaviour
{
    PhotonView pw;
    int saglik = 100;
    public GameObject[] noktalar;
    int CanSayisi;
    int HedefOyuncu;
    float rotationX;
    float rotationY;

    void Start()
    {
        CanSayisi = 10;
        pw = GetComponent<PhotonView>();
       
        if (pw.IsMine)
        {
            GetComponent<Renderer>().material.color = Color.blue;

            if (PhotonNetwork.IsMasterClient)
            {
                transform.position = noktalar[0].transform.position;
                HedefOyuncu = 1;
            }
            else
            {
                transform.position = noktalar[1].transform.position;
                HedefOyuncu = 0;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pw.IsMine)
        {

            
            hareket();
            zipla();
            atesEt();
             // transform.Rotate(Input.mousePosition.x, Input.mousePosition.y, 0 * 10f * Time.deltaTime);

           

              if (Input.GetAxis("Mouse X")<0)
               {
                   transform.Rotate((Vector3.up) * -1.5f);

               }
               if (Input.GetAxis("Mouse X") > 0)
               {
                   transform.Rotate((Vector3.up) * 1.5f);

               }
            
           
        }     

         


    }
    void atesEt()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
           
            if (Physics.Raycast(transform.position,transform.forward, out hit,100f))
            {

                if (hit.transform.gameObject.CompareTag("Dusman"))
                {
                    //  hit.collider.gameObject.GetComponent<PhotonView>().RPC("darbever", RpcTarget.All, 20);
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("CanAzalt", RpcTarget.All, HedefOyuncu);
                }



            }
            

        }

    }

    void hareket()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * 20f);
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * 20f);      

    }

    void zipla()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 0);

        }
        

    }

    [PunRPC]
    void darbever(int darbegucu)
    {
        saglik -= darbegucu;
        Debug.Log("Kalan Sağlık" + saglik);
        if (saglik<=0)
        {
            PhotonNetwork.Destroy(gameObject);

        }
        
    }

    [PunRPC]
    void CanAzalt(int HedefOyuncu)
    {
        CanSayisi--;
       
        GameObject.FindWithTag("SunucuYonetimi").GetComponent<sunucuyonetim>().SkorlariGuncelle(HedefOyuncu, CanSayisi);
    }
}
