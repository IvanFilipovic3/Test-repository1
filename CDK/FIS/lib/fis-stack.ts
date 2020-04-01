import * as cdk from '@aws-cdk/core';
import ec2 = require("@aws-cdk/aws-ec2");
import ecs = require("@aws-cdk/aws-ecs");
import ecs_patterns = require("@aws-cdk/aws-ecs-patterns");
import elbv2 = require('@aws-cdk/aws-elasticloadbalancingv2');


export class FisStack extends cdk.Stack {
  constructor(scope: cdk.Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);


    const vpc = new ec2.Vpc(this, 'VpcForECS', { maxAzs: 2 });

// Craate a cluster
    const cluster = new ecs.Cluster(this, 'EcsCluster', { vpc });
    cluster.addCapacity('DefaultAutoScalingGroup', {
      instanceType: ec2.InstanceType.of(ec2.InstanceClass.T2, ec2.InstanceSize.MICRO)
    });

// Deploy a Docker image
    const taskDefinition = new ecs.Ec2TaskDefinition(this, 'TaskDef');
    const container = taskDefinition.addContainer('web', {
      image: ecs.ContainerImage.fromRegistry("amazon/amazon-ecs-sample"),
      memoryLimitMiB: 256,
    });

    container.addPortMappings({
      containerPort: 80,
      hostPort: 8080,
      protocol: ecs.Protocol.TCP
    });

// Create Service using Docker Container
    const service = new ecs.Ec2Service(this, "Service", {
      cluster,
      taskDefinition,
    });

// Create ALB
    const lb = new elbv2.ApplicationLoadBalancer(this, 'LB', {
      vpc,
      internetFacing: true
    });
    const listener = lb.addListener('PublicListener', { port: 80, open: true });

// Attach ALB to ECS Service
    listener.addTargets('ECS', {
      port: 80,
      targets: [service.loadBalancerTarget({
        containerName: 'web',
        containerPort: 80
      })],
      // include health check (default is none)
      healthCheck: {
        interval: cdk.Duration.seconds(60),
        path: "/health",
        timeout: cdk.Duration.seconds(5),
      }
    });

    new cdk.CfnOutput(this, 'LoadBalancerDNS', { value: lb.loadBalancerDnsName, });


  }
}


