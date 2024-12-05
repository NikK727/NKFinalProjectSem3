using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NKFinalProjectSem3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Song backgroundMusic;

        private enum GameState { Menu, Playing, GameOver }
        private GameState _currentState;

        private Player _player;
        private Texture2D _playerTexture;

        private List<Enemy> _enemies;
        private Texture2D _enemyTexture;

        private List<Coin> _coins;
        private Texture2D _coinTexture;

        private int _score;
        private double _timer;
        private bool _gameOver;

        private double _enemySpawnTimer;
        private double _coinSpawnTimer;
        private const double EnemySpawnInterval = 2.0;
        private const double CoinSpawnInterval = 3.0;

        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _currentState = GameState.Menu;

        }

        protected override void Initialize()
        {

            _player = new Player(new Vector2(200, 200));
            _enemies = new List<Enemy>();
            _coins = new List<Coin>();

            _score = 0;
            _timer = 0;
            _enemySpawnTimer = 0;
            _coinSpawnTimer = 0;
            _gameOver = false;

            base.Initialize();

        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundMusic = Content.Load<Song>("audios/BGM");
            MediaPlayer.Play(backgroundMusic);

            _playerTexture = Content.Load<Texture2D>("images/player");
            _enemyTexture = Content.Load<Texture2D>("images/enemy");
            _coinTexture = Content.Load<Texture2D>("images/coin");

            int playerWidth = 100;
            int playerHeight = 100;

            _playerTexture = ResizeTexture(_playerTexture, playerWidth, playerHeight);
            _enemyTexture = ResizeTexture(_enemyTexture, playerWidth, playerHeight);
            _coinTexture = ResizeTexture(_coinTexture, playerWidth, playerHeight);

            _player.SetTexture(_playerTexture);

        }

        private Texture2D ResizeTexture(Texture2D originalTexture, int newWidth, int newHeight)
        {

            RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, newWidth, newHeight);
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin();
            _spriteBatch.Draw(originalTexture, new Rectangle(0, 0, newWidth, newHeight), Color.White);
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            return renderTarget;

        }

        protected override void Update(GameTime gameTime)
        {

            if (_currentState == GameState.GameOver && Keyboard.GetState().IsKeyDown(Keys.R))
            {

                _currentState = GameState.Playing;
                Initialize();

            }

            if (_currentState == GameState.Menu)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {

                    _currentState = GameState.Playing;
                    Initialize();

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            }

            if (_currentState == GameState.Playing && !_gameOver)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

                _timer += gameTime.ElapsedGameTime.TotalSeconds;
                _enemySpawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
                _coinSpawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

                _player.Update(Keyboard.GetState(), _graphics.GraphicsDevice.Viewport.Bounds);

                if (_enemySpawnTimer >= EnemySpawnInterval)
                {

                    SpawnEnemy();
                    _enemySpawnTimer = 0;

                }

                if (_coinSpawnTimer >= CoinSpawnInterval)
                {

                    SpawnCoin();
                    _coinSpawnTimer = 0;

                }

                foreach (var enemy in _enemies.ToList())
                {

                    enemy.Update();
                    if (enemy.Position.X < -enemy.Bounds.Width) _enemies.Remove(enemy);

                }

                foreach (var coin in _coins.ToList())
                {

                    coin.Update();
                    if (coin.Position.X < -coin.Bounds.Width) _coins.Remove(coin);

                }

                foreach (var enemy in _enemies)
                {

                    if (_player.Bounds.Intersects(enemy.Bounds))
                    {

                        _score -= 1;
                        _gameOver = true;
                        _currentState = GameState.GameOver;
                        break;

                    }

                }

                foreach (var coin in _coins.ToList())
                {

                    if (_player.Bounds.Intersects(coin.Bounds))
                    {

                        _score += 5;
                        _coins.Remove(coin);

                    }

                }

            }

            base.Update(gameTime);

        }

        private void SpawnEnemy()
        {

            var random = new Random();
            int speed = random.Next(2, 6);
            int yPosition = random.Next(0, Math.Max(1, _graphics.PreferredBackBufferHeight - _enemyTexture.Height));
            _enemies.Add(new Enemy(new Vector2(_graphics.PreferredBackBufferWidth, yPosition), _enemyTexture, speed));

        }

        private void SpawnCoin()
        {

            var random = new Random();
            int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight - _coinTexture.Height);
            _coins.Add(new Coin(new Vector2(_graphics.PreferredBackBufferWidth, yPosition), _coinTexture));

        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (_currentState == GameState.Menu)
            {

                _spriteBatch.DrawString(Content.Load<SpriteFont>("MenuFont"), "Press ENTER to Start", new Vector2(300, 200), Color.White);
                _spriteBatch.DrawString(Content.Load<SpriteFont>("MenuFont"), "Press ESC to Exit", new Vector2(300, 300), Color.White);

            }
            else if (_currentState == GameState.Playing)
            {

                _player.Draw(_spriteBatch);
                foreach (var enemy in _enemies) enemy.Draw(_spriteBatch);
                foreach (var coin in _coins) coin.Draw(_spriteBatch);

                _spriteBatch.DrawString(Content.Load<SpriteFont>("ScoreFont"), $"Score: {_score}", new Vector2(10, 10), Color.White);
                _spriteBatch.DrawString(Content.Load<SpriteFont>("ScoreFont"), $"Time: {_timer:F2}", new Vector2(10, 40), Color.White);

            }
            else if (_currentState == GameState.GameOver)
            {

                _spriteBatch.DrawString(Content.Load<SpriteFont>("MenuFont"), "Game Over! Press R to Restart", new Vector2(250, 300), Color.Red);
                _spriteBatch.DrawString(Content.Load<SpriteFont>("MenuFont"), $"Score: {_score}", new Vector2(350, 350), Color.White);

            }

            _spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}