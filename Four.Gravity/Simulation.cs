using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.GeometricAlgebra;
using MathNet.GeometricAlgebra.Constants;
using static MathNet.Extensions.Ergonomy;

namespace Gravity
{
    class Simulation
    {
        public Space Space;
        public List<Particle> Particles;

        public Simulation(uint dimension, ulong particles)
            : this(dimension, particles, new ContinuousUniform(-1, 1), new ContinuousUniform(-1,1)) { }

        public Simulation(uint dimension, ulong particles, IDistribution positionDistribution, IDistribution velocityDistribution)
        {
            Space = new Space(dimension);

            
            for (ulong i = 0; i < particles; i++)
            {
                var particle = new Particle(this);

                for (int j = 0; j < dimension; j++)
                {
                    if(positionDistribution != null)
                        particle.Position.Elements[j] = positionDistribution.RandomSource.NextDouble();

                    if(velocityDistribution != null)
                        particle.Velocity.Elements[j] = velocityDistribution.RandomSource.NextDouble();
                }
            }
        }
    }


    class Particle
    {
        public Simulation Simulation;
        public PureGrade Position;
        public PureGrade Velocity;
        public double Mass = 1;

        public PureGrade Momentum
        {
            get => Mass * Velocity;
            set => Velocity.Copy(value / Mass);
        }

        public double KineticEnergy
        {
            get => 0.5 * Mass * Sq(Velocity.L2Norm);
        }

        public Particle(Simulation simulation)
        {
            Simulation = simulation;
            Position = new PureGrade(simulation.Space, 1);
            Velocity = new PureGrade(simulation.Space, 1);
        }
    }
}
