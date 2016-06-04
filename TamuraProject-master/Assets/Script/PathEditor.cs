using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class PathEditor : MonoBehaviour {
    [SerializeField]
    private bool editMode = false;
    [SerializeField]
    private GameObject pointSample;

    public List<PathPoint> Path { get { return Points; } }
    public event Action OnPathLoaded;

    public class PathPoint
    {
        public GameObject Base;
        public List<PathPoint> ConnectPoint;
    }
    List<PathPoint> Points = new List<PathPoint>();

    PathPoint Editing;

    [Serializable]
    class PathFile
    {
        public float x;
        public float y;
        public List<int> Connects;
    }

    void Start()
    {
        BuildFromData(LoadFile(name + ".path"));
    }

    void Update()
    {
        if (editMode)
        {
            Points.ForEach(p => p.Base.SetActive(true));

            float dx = Input.GetAxis("Horizontal");
            float dy = Input.GetAxis("Vertical");
            Camera.main.transform.Translate(dx, dy, 0.0F);

            Points.ForEach(p =>
            {
                p.ConnectPoint.ForEach(c =>
                {
                    Debug.DrawLine(p.Base.transform.position, c.Base.transform.position, Color.black);
                });
            });
        }
        else
        {
            if (Points.Exists(p => p.Base.activeSelf))
            {
                Points.ForEach(p => p.Base.GetComponent<SpriteRenderer>().color = Color.white);
                Editing = null;
                SaveFile(BuildToData());
            }

            Points.ForEach(p => p.Base.SetActive(false));
        }
    }

    List<PathFile> LoadFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            try
            {
                FileStream oFileStream = File.OpenRead(fileName);
                BinaryFormatter f = new BinaryFormatter();
                List<PathFile> data = f.Deserialize(oFileStream) as List<PathFile>;
                oFileStream.Close();
                return data;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        return null;
    }

    void BuildFromData(List<PathFile> data)
    {
        if (data != null && data.Count > 0)
        {
            data.ForEach(c =>
            {
                PathPoint point = CreateNewPoint(null);
                RegisterPoint(point.Base.GetComponent<PathPointControl>());
                point.Base.transform.position = new Vector3(c.x, c.y, 0);
            });

            Points.ForEach(p =>
            {
                int index = Points.IndexOf(p);
                if (index < data.Count)
                {
                    data[index].Connects.ForEach(c =>
                    {
                        if (c < Points.Count && c != index)
                        {
                            p.ConnectPoint.Add(Points[c]);
                        }
                    });
                }
            });
        }
        else
        {
            CreateNewPoint(null);
        }

        OnPathLoaded();
    }

    List<PathFile> BuildToData()
    {
        if (Points.Count > 0)
        {
            List<PathFile> data = new List<PathFile>();

            Points.ForEach(p =>
            {
                PathFile tmp = new PathFile() { x = p.Base.transform.position.x, y = p.Base.transform.position.y, Connects = new List<int>() };
                p.ConnectPoint.ForEach(c =>
                {
                    tmp.Connects.Add(Points.IndexOf(c));
                });

                data.Add(tmp);
            });

            return data;
        }

        return null;
    }

    void SaveFile(List<PathFile> data)
    {
        if (data != null && data.Count > 0)
        {
            try
            {
#if UNITY_IPHONE
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif
                using (FileStream fs = new FileStream(name + ".path", FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, data);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }

    public void RegisterPoint(PathPointControl ctl)
    {
        if (ctl != null)
        {
            ctl.OnClick += PointClicked;
            ctl.OnDelete += PointDeleted;
            ctl.OnCreate += PointCreated;
        }
    }

    PathPoint CreateNewPoint(GameObject obj)
    {
        if (obj == null)
        {
            obj = Instantiate(pointSample);
        }
        else if (obj.GetComponent<PathPointControl>() == null)
        {
            return null;
        }

        obj.transform.parent = transform;

        PathPoint newPoint = new PathPoint() { Base = obj, ConnectPoint = new List<PathPoint>() };

        Points.Add(newPoint);

        return newPoint;
    }

    void PointCreated(GameObject obj)
    {
        PathPoint point = Points.Find(p => p.Base == obj);
        if (point == null)
        {
            CreateNewPoint(obj);
        }
    }

    void PointClicked(GameObject obj)
    {
        if (!editMode)
        {
            return;
        }

        PathPoint point = Points.Find(p => p.Base == obj);
        if (point != null)
        {
            if (Editing == null)
            {
                Editing = point;
                point.Base.GetComponent<SpriteRenderer>().color = Color.red;
                point.ConnectPoint.ForEach(c => c.Base.GetComponent<SpriteRenderer>().color = Color.green);
            }
            else
            {
                if (point == Editing)
                {
                    Editing = null;
                    point.Base.GetComponent<SpriteRenderer>().color = Color.white;
                    point.ConnectPoint.ForEach(c => c.Base.GetComponent<SpriteRenderer>().color = Color.white);
                }
                else
                {
                    if (Editing.ConnectPoint.Exists(c => c == point))
                    {
                        point.Base.GetComponent<SpriteRenderer>().color = Color.white;
                        Editing.ConnectPoint.Remove(point);
                        point.ConnectPoint.Remove(Editing);
                    }
                    else
                    {
                        point.Base.GetComponent<SpriteRenderer>().color = Color.green;
                        Editing.ConnectPoint.Add(point);
                        point.ConnectPoint.Add(Editing);
                    }
                }
            }
        }
    }

    void PointDeleted(GameObject obj)
    {
        PathPoint point = Points.Find(p => p.Base == obj);
        if (point != null)
        {
            point.ConnectPoint.ForEach(c => c.ConnectPoint.Remove(point));
            Points.Remove(point);
        }
    }
}
