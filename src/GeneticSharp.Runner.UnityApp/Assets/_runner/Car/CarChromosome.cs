using GeneticSharp.Domain.Chromosomes;
using UnityEngine;
using GeneticSharp.Domain.Randomizations;
using System.Linq;

namespace GeneticSharp.Runner.UnityApp.Car
{
    public class CarChromosome : ChromosomeBase
    {
        private CarSampleConfig m_config;
        private float m_angle;

        public CarChromosome(CarSampleConfig config) : base(config.VectorsCount)
        {
            m_config = config;
            m_angle = 360f / config.VectorsCount;

            for (int i = 0; i < Length; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }
        }

        public string ID { get; } = System.Guid.NewGuid().ToString();

        public bool Evaluated { get; set; }
        public float MaxDistance { get; set; }
        public float MaxDistanceTime { get; set; }
      
        public override IChromosome CreateNew()
        {
            return new CarChromosome(m_config);
        }

        public override Gene GenerateGene(int geneIndex)
        {

            CarGeneValue value;

            if (geneIndex < m_config.WheelsCount)
            {
                value = new CarGeneValue(
                    GetRandomVectorSize(), 
                    GetRandomVectorAngle(),
                    GetRandomWheelIndex(), 
                    GetRandomWheelRadius());
            }
            else
            {
                value = new CarGeneValue(GetRandomVectorSize(), GetRandomVectorAngle());
            }


            return new Gene(value);
   
        }

        public CarGeneValue[] GetGenesValues()
        {
            return GetGenes().Select(g => (CarGeneValue)g.Value).ToArray();
        }

        float GetRandomVectorSize()
        {
            return RandomizationProvider.Current.GetFloat(1, m_config.MaxVectorSize);
        }

        float GetRandomVectorAngle()
        {
            return RandomizationProvider.Current.GetFloat(0, 360);
        }

        int GetRandomWheelIndex()
        {
            return RandomizationProvider.Current.GetInt(0, m_config.VectorsCount);
        }

        float GetRandomWheelRadius()
        {
            // The range is form negative to positive MaxWheelRadius, because we want the same probability to have a 
            // point with a wheel and without one.
            var radius =  RandomizationProvider.Current.GetFloat(-m_config.MaxWheelRadius, m_config.MaxWheelRadius);

            return radius > 0 ? radius : 0;
        }

        public Vector2 GetVector(int geneIndex, CarGeneValue geneValue)
        {
            var rnd = RandomizationProvider.Current;
        
            // GeneValue is the radius.
            var offset = new Vector2(Mathf.Sin(geneValue.VectorAngle), Mathf.Cos(geneValue.VectorAngle)) * geneValue.VectorSize;
            return Vector2.zero + offset;
        }
    }
}