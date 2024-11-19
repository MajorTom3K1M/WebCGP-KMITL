'use client'

import Image from 'next/image';
import { useState } from "react";
import { Search, Play, Globe, Calendar, User, Tag, Lock } from "lucide-react";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";

import FullScreenImage from "@/components/FullScreenImage";


const games = [
  {
    id: 1,
    title: "Mass Effect",
    image: "/games/mass-effect.jpg",
    banner: "/games/mass-effect.jpg",
    description: "Embark on an interstellar journey, exploring new planets and building your own space empire in this exciting browser-based strategy game.",
    releaseDate: "May 14, 2023",
    developer: "Cosmic Games",
    publisher: "Web Gaming Co.",
    genres: ["Strategy", "Sci-Fi", "Multiplayer"],
    playerCount: "1-1000 players",
    platform: "WebGL",
    controls: "Mouse & Keyboard",
    rating: "E for Everyone",
    status: "Coming Soon",
    screenshots: [
      "/games/mass-effect-screenshot-2.png",
      "/games/mass-effect-screenshot-1.jpg",
      "/games/mass-effect-screenshot-3.jpeg",
      "/games/mass-effect-screenshot-5.jpg"
    ],
    tutorial: () => {
      return (
        <>
          <div>
            <div className="font-medium">Controls:</div>
            <div className="text-muted-foreground">Mouse & Keyboard</div>
          </div>
          <div>
            <div className="font-medium">Quick Start:</div>
            <ol className="list-decimal list-inside text-muted-foreground">
              <li>Click the &quot;Play Now&quot; button</li>
              <li>Allow the game to load in your browser</li>
              <li>Follow the in-game tutorial</li>
              <li>Enjoy your gaming experience!</li>
            </ol>
          </div>
        </>
      )
    }
  },
  {
    id: 2,
    title: "GEMyth",
    image: "/games/ge-myth-banner-2.webp",
    banner: "/games/ge-myth-banner-2.webp",
    description: "Puzzle Game using color macthing mechanics. On each play, gem miner have to shoot a gem to match color of the gem cluster. Once player has cleared all of the gems, Miner wins. But if the gem cluster have hit the danger zone, Miner lose.",
    releaseDate: "May 14, 2024",
    developer: "TEAM MAJOX",
    publisher: "JKRN Co.",
    genres: ["Strategy", "Puzzle", "Singleplayer"],
    playerCount: "1 players",
    platform: "WebGL",
    controls: "Mouse & Keyboard",
    rating: "E for Everyone",
    link: "http://localhost:5003",
    // link: "http://puzzlebobble:80",
    screenshots: [
      "/games/ge-myth-screenshot-1.png",
      "/games/ge-myth-screenshot-2.png",
      "/games/ge-myth-screenshot-3.png",
    ],
    tutorial: () => {
      return (
        <>
          <div>
            <div className="font-medium">Controls:</div>
            <div className="text-muted-foreground">Mouse & Keyboard</div>
            <div className="text-muted-foreground">Click to Shoot</div>
          </div>
          <div>
            <div className="font-medium">Quick Start:</div>
            <ol className="list-decimal list-inside text-muted-foreground">
              <li>Click the &quot;Play Now&quot; button</li>
              <li>Allow the game to load in your browser</li>
              <li>Enjoy your gaming experience!</li>
            </ol>
          </div>
        </>
      )
    }
  },
  {
    id: 3,
    title: "Wizardous",
    image: "/games/wizardious-banner.webp",
    banner: "/games/wizardious-banner.webp",
    description: `This game mainly bases on 2-D Catapult Game with additional mechanics for example Side-Scrolling, Turn Based.
Player will be played as a Wizard with an objective of this following list 
- To traverse on each level
- To eliminate all enemy and obstacle on each level
Also, the game will show a hint or any action as a Storytelling for player to improvise your own story.`,
    releaseDate: "February 10, 2024",
    developer: "TEAM MAJOX",
    publisher: "JKRN Co.",
    genres: ["Strategy", "Side Scrolling", "Singleplayer"],
    playerCount: "1 players",
    platform: "WebGL",
    controls: "Mouse & Keyboard",
    rating: "E for Everyone",
    link: "http://localhost:5004",
    // link: "http://wizardious:80",
    screenshots: [
      "/games/wizardous-screenshot-1.png",
      "/games/wizardous-screenshot-2.png",
      "/games/wizardous-screenshot-3.png",
    ],
    tutorial: () => {
      return (
        <>
          <div>
            <div className="font-medium">Controls:</div>
            <div className="text-muted-foreground">Keyboard</div>
            <div className="text-muted-foreground">SpaceBar to Shoot</div>
            <div className="text-muted-foreground">Number to Use Item</div>
            <div className="text-muted-foreground">Q/W/E to Use Skill</div>
          </div>
          <div>
            <div className="font-medium">Quick Start:</div>
            <ol className="list-decimal list-inside text-muted-foreground">
              <li>Click the &quot;Play Now&quot; button</li>
              <li>Allow the game to load in your browser</li>
              <li>Enjoy your gaming experience!</li>
            </ol>
          </div>
        </>
      )
    }
  },
  {
    id: 4,
    title: "Chess",
    image: "/games/chess-banner.webp",
    banner: "/games/chess-banner.webp",
    description: "Experience the timeless strategy of chess with our Classic Two-Player Chess Game. Perfect for friends and family, this authentic set features traditional pieces and an 8x8 board, providing endless opportunities for tactical battles and intellectual growth. Whether you're a beginner or a seasoned player, enjoy hours of competitive fun and sharpen your strategic thinking skills.",
    releaseDate: "June 18, 2024",
    developer: "TEAM MAJOX",
    publisher: "JKRN Co.",
    genres: ["Strategy", "Board Game", "Multiplayer"],
    playerCount: "1-2 players",
    platform: "WebGL",
    controls: "Mouse & Keyboard",
    rating: "E for Everyone",
    link: "http://localhost:5001",
    // link: "http://chess:80",
    screenshots: [
      "/games/chess-screenshot-1.png",
      "/games/chess-screenshot-2.png",
    ],
    tutorial: () => {
      return (
        <>
          <div>
            <div className="font-medium">Controls:</div>
            <div className="text-muted-foreground">Mouse</div>
            <div className="text-muted-foreground">Click to Move</div>
          </div>
          <div>
            <div className="font-medium">Quick Start:</div>
            <ol className="list-decimal list-inside text-muted-foreground">
              <li>Click the &quot;Play Now&quot; button</li>
              <li>Allow the game to load in your browser</li>
              <li>Enjoy your gaming experience!</li>
            </ol>
          </div>
        </>
      )
    }
  },
  {
    id: 5,
    title: "Breakout",
    image: "/games/breakout-image.webp",
    banner: "/games/breakout-banner.webp",
    description: "Dive into the classic arcade action of Breakout! Control a paddle to bounce a ball and demolish colorful brick layers. With increasing levels, power-ups, and addictive gameplay, Breakout offers endless fun for players of all ages. Challenge yourself to achieve high scores and master the art of strategic ball control!",
    releaseDate: "October 5, 2024",
    developer: "TEAM MAJOX",
    publisher: "JKRN Co.",
    genres: ["Arcade", "Brick Breaker"],
    playerCount: "1 players",
    platform: "WebGL",
    controls: "Mouse & Keyboard",
    rating: "E for Everyone",
    link: "http://localhost:5002",
    // link: "http://breakout:80",
    screenshots: [
      "/games/breakout-screenshot-1.png",
    ],
    tutorial: () => {
      return (
        <>
          <div>
            <div className="font-medium">Controls:</div>
            <div className="text-muted-foreground">Keyboard</div>
            <div className="text-muted-foreground">Left/Right Arrow to Move the Platform</div>
          </div>
          <div>
            <div className="font-medium">Quick Start:</div>
            <ol className="list-decimal list-inside text-muted-foreground">
              <li>Click the &quot;Play Now&quot; button</li>
              <li>Allow the game to load in your browser</li>
              <li>Enjoy your gaming experience!</li>
            </ol>
          </div>
        </>
      )
    }
  },
]

export default function Component() {
  const [selectedGame, setSelectedGame] = useState(games[0]);
  const [searchQuery, setSearchQuery] = useState("");
  const [fullScreenImage, setFullScreenImage] = useState<string | null>(null);

  const filteredGames = games.filter(game =>
    game.title.toLowerCase().includes(searchQuery.toLowerCase())
  )

  const navigateToExternalSite = (link?: string) => {
    window.location.href = link ?? "";
  };

  return (
    <div className="flex h-screen bg-[#1a1a1a] text-white">
      {/* Sidebar */}
      <div className="w-80 bg-[#0d0d0d] p-4 flex flex-col">
        <h1 className="text-2xl font-bold mb-4">Game Library</h1>
        <div className="relative mb-4">
          <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input
            type="text"
            placeholder="Search games"
            className="pl-8 bg-[#2a2a2a] border-none"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>
        <div className="space-y-2 overflow-auto">
          {filteredGames.map((game) => (
            <button
              key={game.id}
              onClick={() => setSelectedGame(game)}
              className={`flex items-center space-x-3 w-full p-2 rounded-lg transition-colors ${selectedGame.id === game.id ? "bg-[#2a2a2a]" : "hover:bg-[#2a2a2a]"
                }`}
            >
              <Image src={game.image} alt={game.title} className="object-cover w-12 h-12 rounded" />
              <div className="text-left">
                <div className="font-medium">{game.title}</div>
                {game.status && (
                  <Badge variant="secondary" className="mt-1">
                    {game.status}
                  </Badge>
                )}
              </div>
            </button>
          ))}
        </div>
      </div>

      {/* Main Content */}
      <div className="flex-1 overflow-auto">
        {selectedGame && (
          <div>
            {/* Banner */}
            <div className="relative h-[400px]">
              <Image
                src={selectedGame.banner}
                alt={selectedGame.title}
                className="w-full h-full object-cover"
              />
              <div className="absolute bottom-0 left-0 right-0 p-6 bg-gradient-to-t from-black to-transparent">
                <h2 className="text-4xl font-bold mb-2">{selectedGame.title}</h2>
                <Button
                  onClick={() => navigateToExternalSite(selectedGame?.link)}
                  className={`${
                    selectedGame.status === "Coming Soon" 
                      ? "bg-gray-600 hover:bg-gray-700 cursor-not-allowed" 
                      : "bg-blue-600 hover:bg-blue-700"
                  } text-white`}
                  disabled={selectedGame.status === "Coming Soon"}
                >
                  {selectedGame.status === "Coming Soon" ? (
                    <>
                      <Lock className="mr-2 h-4 w-4" />
                      Coming Soon
                    </>
                  ) : (
                    <>
                      <Play className="mr-2 h-4 w-4" />
                      Play Now
                    </>
                  )}
                </Button>
              </div>
            </div>

            {/* Game Info */}
            <div className="p-6">
              <div className="grid grid-cols-3 gap-8">
                <div className="col-span-2">

                  <div className="bg-white bg-opacity-10 backdrop-filter backdrop-blur-lg rounded-xl p-6 mb-6">
                    <div className="flex items-start space-x-4 mb-4">
                      <Image src={selectedGame.image} alt={selectedGame.title} className="object-cover w-24 h-24 rounded-lg" />
                      <div>
                        <h3 className="text-2xl font-semibold mb-2">{selectedGame.title}</h3>
                        <p className="text-muted-foreground" style={{ whiteSpace: 'pre-line' }}>{selectedGame.description}</p>
                      </div>
                    </div>
                    <div className="grid grid-cols-2 gap-4">
                      <div className="flex items-center space-x-2">
                        <Calendar className="h-4 w-4 text-muted-foreground" />
                        <div>
                          <div className="font-medium">Release Date:</div>
                          <div className="text-muted-foreground">{selectedGame.releaseDate}</div>
                        </div>
                      </div>
                      <div className="flex items-center space-x-2">
                        <User className="h-4 w-4 text-muted-foreground" />
                        <div>
                          <div className="font-medium">Developer:</div>
                          <div className="text-muted-foreground">{selectedGame.developer}</div>
                        </div>
                      </div>
                      <div className="flex items-center space-x-2">
                        <User className="h-4 w-4 text-muted-foreground" />
                        <div>
                          <div className="font-medium">Publisher:</div>
                          <div className="text-muted-foreground">{selectedGame.publisher}</div>
                        </div>
                      </div>
                      <div className="flex items-center space-x-2">
                        <Globe className="h-4 w-4 text-muted-foreground" />
                        <div>
                          <div className="font-medium">Platform:</div>
                          <div className="text-muted-foreground">{selectedGame.platform}</div>
                        </div>
                      </div>
                    </div>
                  </div>

                  <div className="mb-6">
                    <h3 className="text-lg font-semibold mb-2">Genres:</h3>
                    <div className="flex gap-2">
                      {selectedGame.genres?.map((genre) => (
                        <Badge key={genre} variant="secondary">
                          {genre}
                        </Badge>
                      ))}
                    </div>
                  </div>

                  <div className="grid grid-cols-2 gap-4 mb-6">
                    <div className="flex items-center space-x-2">
                      <User className="h-4 w-4 text-muted-foreground" />
                      <div>
                        <div className="font-medium">Player Count:</div>
                        <div className="text-muted-foreground">{selectedGame.playerCount}</div>
                      </div>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Tag className="h-4 w-4 text-muted-foreground" />
                      <div>
                        <div className="font-medium">Age Rating:</div>
                        <div className="text-muted-foreground">{selectedGame.rating}</div>
                      </div>
                    </div>
                  </div>

                  {/* Screenshots */}
                  <div>
                    <h3 className="text-lg font-semibold mb-4">Screenshots</h3>
                    <div className="grid grid-cols-2 gap-4">
                      {selectedGame.screenshots?.map((screenshot, index) => (
                        <button
                          key={index}
                          onClick={() => setFullScreenImage(screenshot)}
                          className="relative group"
                        >
                          <Image
                            src={screenshot}
                            alt={`${selectedGame.title} screenshot ${index + 1}`}
                            className="w-full h-48 object-cover rounded-lg"
                          />
                        </button>
                      ))}
                    </div>
                  </div>
                </div>

                <div>
                  <h3 className="text-lg font-semibold mb-4">How to Play</h3>
                  <div className="space-y-4">
                    {selectedGame.tutorial()}
                    {/* <div>
                      <div className="font-medium">Controls:</div>
                      <div className="text-muted-foreground">{selectedGame.controls}</div>
                    </div>
                    <div>
                      <div className="font-medium">Quick Start:</div>
                      <ol className="list-decimal list-inside text-muted-foreground">
                        <li>Click the "Play Now" button</li>
                        <li>Allow the game to load in your browser</li>
                        <li>Follow the in-game tutorial</li>
                        <li>Enjoy your gaming experience!</li>
                      </ol>
                    </div> */}
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>

      {fullScreenImage && (
        <FullScreenImage
          src={fullScreenImage}
          alt="Full screen screenshot"
          onClose={() => setFullScreenImage(null)}
        />
      )}
    </div>
  )
}