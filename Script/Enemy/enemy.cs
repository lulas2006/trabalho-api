using Godot;
using System;

public partial class enemy : CharacterBody2D
{
	public const float Speed = 150f;

	Vector2 playerPosition;
	Vector2 mobPosition = Vector2.Zero;
	Vector2 targetPosition = Vector2.Zero;
	
	public AnimationPlayer animationPlayer;
	public Timer timer;
	
	bool player_in_att_zone = false; 
	public bool en_attack_cooldown = true;
	public int enHealth = 10;
	player Player = new player();
	
	public override void _PhysicsProcess(double delta)
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		
		Vector2 velocity = Velocity;
		mobPosition = this.Position;
		playerPosition = GetNode<CharacterBody2D>($"../Player").Position;
		targetPosition = (playerPosition - mobPosition).Normalized();

		velocity = Vector2.Zero;

		if (mobPosition.DistanceTo(playerPosition) < 50000) {
			velocity = targetPosition;
			animationPlayer.Play("Run");
			if(velocity.X < 0) {
				animationPlayer.Play("Run");
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
			} else if (velocity.X > 0) {
				animationPlayer.Play("Run");
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
			}
		} else {
			animationPlayer.Play("Idle");
		}

		velocity = velocity * Speed;
		Velocity = velocity;
		
		MoveAndSlide();
	}
	
	private void _on_enemy_hit_box_body_entered(Node2D body)
	{
		if(body.Name == "Player") {
			player_in_att_zone = true;
		}
	}

	private void _on_enemy_hit_box_body_exited(Node2D body)
	{
		if(body.Name == "Player") {
			player_in_att_zone = false;
		}
	}

	private void SpawnCoin()
	{
		var scene = GD.Load<PackedScene>("res://path/to/CoinScene.tscn");
		var coin = scene.Instantiate<coin>();
			AddChild(coin);
		coin.Position = Position;
	}

	private void OnEnemyDeath()
	{
		// Your existing code for enemy death...

		SpawnCoin();
		QueueFree();
	}
}

