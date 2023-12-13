
//MY NOTATIONS


//i need to create less dependecies between stuff.
//start using playfab maybe.





//CHANGES TO DESIGN

//wheen you talk with npcs you dont have to give anything.
//you instantly roll.
//things can you craft wwith are not sellable and things that are sellable you cannot craft with.


//seed - only in planter and its not an actual item
//plant - result of seed. important because all potions use some plant.
//ingredients - things acquired in dungeons to make better potions.
//potion - the only thing sellable.
//minerals and crystals - not actual item. are used for improveements and buying stuff.
//equippment - not an actual item. used for equipping

//my problem is about display a lot of inventory. limit it.
//it needs ot be fast and to the least amount of thinking necessary. its an auto game.
//


//HOW OBJECTS WILL WORK:
//planter - does not open ui. instead you buy the place and they keep producing. to unluck the field you require the seed. now its an actual item.
//itemholder - open the sellable ui. click and pass the whole stack to the holder. no choosing of exact quantitys
//productions - click to autocollect wwhats inside. to choose production. choose production from a limited possibilities.
//seller - is just get there and sell before clients get tired of waiting.
//


//sell the stuff from the dungeon. a half and half game?
//should aim for infinite game.

//HOW RAID WILL WORK
//its still auto
//simplify the equips and everything because it cannot be overwhelming. 
//you feed them the food they like to increase their level. you get them through cards from chest but only the first one matters.
//they have two weapon slots. they can be improved in the smith. 


//OUTSIDE
//visit smith
//minigames for prizes.
//improve the village so you get better clients.
//buy other places to get things or passive income.
//start a delivery system which are basically potion that you have to create.


//LONG TIME GOALS
//buying everything in the store.
//improve neighboorhood.
//beat all stages of raid.
//automatizing the store.

//CHAR UPGRADES
//

//IDEAS
//Plant that you need to interact everyday to make it grow.
//



//FIRST
//character
///can buy stuff for the store. itemholder and planter of different typs
///can hold items
///can deliver stuff accordingly.

//SECOND
///get npc working
///npc can buy stuff.
///npc can go and buy stuff and form lines till player uses it.
///npcs have behavior.
///you can talk with npcs.

//THIRD
//player can plant stuff. it grows even when the game is closed based in a server time.
//player can fabricate stuff. same thing here.
//player has one chest he can store stuff.
//i needd to do serveer. otherwise it will keep being a problem.

//THIRD-SERVERANDSAVING
//save information: (player level, player money, player diamond, player hand list, player chest list, store objects bought, objcets lists, player reputation, champion list, equip list, champion equipment and level list)
//store that information only in the server
//ui for creating an account. auto logging into the account based on device if the player chooses to be rememebred
//awards for logging in.
//

//FOURTH
//raids
//have champions. they have levels and be improvedd through equip.
//the raids have enemies.
//can choose your team and each have abilities.
//in the end everyone gain xp and random loot. depending on the loot it goes to 

//FIFTH
//can hire staff. 
//seller that just automates selling the stuff.
//planter that takes care of stuff.
//differnt npcs.
//can improve stuff.
//


//THINGS I SHOULD DO BEFORE DEALING WITH THEE SERVER
//champions list, champion progress and equip, and thee equip list. 
//planter.
//fabricator.
///chest
//combat
//I AM DONE



//TO FIX
///usee the iinventory system to grab things but the problem is that it needsd to take into account the item coming or the player will be able to add more.
///fix the button. it showing grab but when the player clicks the player givees insintead.
///also the secondary option shouldnt appear when there is only one option on what to do.



//CURRENT GOAL
///open champ ui.
///can seleect champ
///can open raid.
///can select a world
///display for stages and selection have effects.
//get especific info from current stage.
///can select a team from your owned and unselected champs 
///able to drag and drop the champions. or to click the champions.
//can then confirm the current champs to then load the map.

//NEXT GOAL
//get production done.
///it show all hand units. but show the difference between the ones you cannot use, the ones that cannot be used due to recipe and the ones that can 
///close ui when moves.
//the reipes update as you put and take itens
//click a button and locks all ingredients til everything is done.
//when done you can just get stuff.


//CURRENT GOAL - RAID
//crating an ability system
//create an auto attack that shoots a projectil
//craete an abiity that shoots another projectil. 
//have cooldown being registered.
//create stats system.
//create a passive that grants attackspeed to a cap after every unit killed.
//create targetting system.


//NEXT GOAL - RAID
//get the stats working
//get the abilities working
//can equip champs in the champui.


//CREATING ABILITIES
//everytime you kill an enemy gain a buff that stacks to a limt and then decays.
//shoot a small arrow.
//shoot a big arrow that explodes.

//TO FIX
//the raid ui is not that good. slow for some reason fix that.


//GOAL 
//an start the raid

//STARTING THE RAID
//


//GOAL 
//can end the raid

//FIX 
///for some reason the itens in hand are appearing somewhere else.

//GOAL FOR TOMORROW AND AFTER
//can start the raid and load with teh selected champs. drag them into it.
//can grab itens
//can simulate ending the raid with just a button.
//the champ shows the exp gained.
//show the itens (no need to organizing). but do a quick animation for each.
//there is a problem about the camera and the two players.
//result -> champ -> inventory -> buttons (all should take less than 15 seconds)

//TASKS 1
///fix the champ ui
//upgrade the champ by spending copies.
//get chests and itens for raid that are taken to the store
//create ui for inventory in raid.
//create system for ending raid and returning to store.



//TASKS 2
//finish the loop of itens (farm, raid, production, selling)
//create the system for the npc
//finish the system for placing the craft and getting stuff done (ui and results)

//TASKS 3
//Create the system for the support champ.
//fix that the targetting should always priority people closer.
//



//TOTAL GOAL 
//3 months of development (16/11/23 - 16/2/23) - about more 90 days of dev now.
//money goal: 3.000 dolar over a year. (low goal)
//


//NEXT PROJECTS
//PET (20/2/23 - 1/4/23)
//GAME DEV (5/4/23 - 25/5/23)
//HORDE SURVIVOR 
//


//ACTUAL PROJECTS THAT ARE POSSIBLE
//Labyrinth project
//Evil project
//Doom project
//



//THINGS TO FIX FOR IMPROVEMENT SAKE
//i dont want to check every list to find the bdbooleans.
//i dont want to contisounly check the value for movespeed.



//Create Potion
//
//INGREDIENTS
//apple - these two are the first basic things you can grow
//banana - 
//Coffe - 
//Fairy dust - 
//Skeleton bones
//dragon feather
//Slime Eye
//
//POTIONS (Drink, Medicinal, Enhancing)
//Refreshment Potion (apple, apple, apple)
//Refreshment potion 2 (apple, apple, banana)
//Sleep Potion (Apple, apple, FairyDust)
//Speed Potion (Apply, Coffee, FairyDust)
//Strenght Potion (Apple, Skeleton Bone, Skeleton Bone)
//Fire Resistance Potion (Apple, Apple, Dragon Feather)
//Healing Potion (Apple, Apple, SlimeEyes)