// Main Contributor: Vin
// Secondary Contributor: Gabriel Heiser
// Reviewer: Gabriel Heiser
// Description: Controller to show health, ammo, and upgrades on the HUD

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    //// Pulic variables ////
    public static HUDController instance;

    [Header("Runtime variables")]
    // Controls the speed of the weapon walking animation
    [SerializeField] private float _weaponAnimSpeed = 1;
    // Controls the amount of movement of the weapon animation
    [SerializeField] private float _weaponAnimAmp = 1;
    [SerializeField] private float zRecoilRotationAmt = 5;
    [SerializeField] private float xRecoilTransformAmt = 3;
    [SerializeField] private float recoilAnimSpeed = 1;
    [SerializeField] private int recoilAnimFrames = 24;
    [SerializeField] private float damageAnimLen = 0.5f;
    [SerializeField] private int damageIndicatorMaxAlpha = 50;
    [SerializeField] private Color[] upgradeColors = {
        new Color(1, 0.5f, 0.5f),   // Armor
        new Color(0.5f, 1, 0.5f),   // Stim
        new Color(0.5f, 0.6f, 1),   // Invisibility
        new Color(1, 0.8f, 0.4f)    // Max Ammo
    };

    [Header("HUD")]
    public Image healthBar;
    public Image damageIndicator;
    public Image cooldownBar;
    public Image tempPickupCooldownBar;
    public Image tempPickupCooldownBG;
    public Image hudTint;
    public TMP_Text tempPickupText;
    public GameObject pickupNotice;
    // Reference bug death counter
    public TextMeshProUGUI bugDeathCounter;
    //total ammo
    public GameObject loadoutAmmoContainer;
    //mag ammo
    public GameObject magazineAmmoContainer;
    //current weapon
    public GameObject weaponSpriteContainer;
    //held weapons icons
    public GameObject weaponIconContainer;
    //Backgrounds behind held weapon
    public GameObject IconBGContainer;
    //upgrade dots next to held weapon icons
    public GameObject upgradeDotContainer;
    //comment later goober
    public Image[] upgradeSlots = new Image[9];
    // References the text boxes for light, medium, and heavy ammo
    public TextMeshProUGUI[] inventoryAmmo = new TextMeshProUGUI[3];


    //// Private variables ////
    // Types of ammo stored in list
    private string[] ammoString = new string[3];
    // Ammo caps stored in list
    private int[] ammoCaps = new int[3];
    // Upgrade text stored in list
    private string[] upgradesString = new string[4];
    private Vector3 weaponStartPos;
    private bool indicatingDamage;
    private float playerMoveDistance = 0;
    private Coroutine currentCooldown;
    private Coroutine currentRecoilCoroutine;
    private Coroutine currentTempCoroutine;
    private Color healthBarColor;
    private Color bulletColor;
    private Image currentFirePoint;



    // Used to make an instance
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Populate ammoString with types of ammo
        ammoString[0] = "Light  ";
        ammoString[1] = "Medium";
        ammoString[2] = "Heavy";
        // Populate ammoCaps with amounts of max ammo
        ammoCaps[0] = 60;
        ammoCaps[1] = 260;
        ammoCaps[2] = 40;

        SetUpgrades();
        ammoCaps = InventoryManager.instance.playerInventory.AMMO_CAPS;
        // Populate upgradesString with upgrade types
        upgradesString[0] = "Invulnerability Armor";
        upgradesString[1] = "Stimulant";
        upgradesString[2] = "Invisibility Shield";
        upgradesString[3] = "Ammo Pack";

        // Make sure upgrade tints are disabled
        tempPickupCooldownBar.enabled = false;
        tempPickupCooldownBG.enabled = false;
        tempPickupText.enabled = false;
        hudTint.enabled = false;

        // Save original color of the healthbar for later restoration
        healthBarColor = healthBar.color;

        bulletColor = Color.white;

        weaponStartPos = weaponSpriteContainer.GetComponent<RectTransform>().position;
        Debug.Log("WeaponStartPos: " + weaponStartPos);
    }

    // Setting bug death counter
    public void SetBugDeathCount(int bugDeathCount, int bugDeathCap)
    {
        bugDeathCounter.text = bugDeathCount.ToString() + "/" + bugDeathCap.ToString();
        Debug.Log(bugDeathCount.ToString() + "/" + bugDeathCap.ToString());
    }

    // Setting max health
    public void SetMaxHealth()
    {
        healthBar.fillAmount = 1;
    }

    // Setting health (used when health is added or subtracted)
    public void DisplayHealth(float health)
    {
        healthBar.fillAmount = health / 100;
    }

    // Update ammo inventory after shooting/reloading
    public void UpdateAmmo(WeaponTemplate.AmmoType ammoType)
    {
        loadoutAmmoContainer.transform.GetChild((int)ammoType).GetComponent<TMP_Text>().text = InventoryManager.instance.playerInventory.GetAmmo(ammoType).ToString();
    }

    // Set ammo inventory to start with strings
    public void DisplayInventoryAmmo(int[] Ammo)
    {
        for (int i = 0; i < 3; i++)
        {
            loadoutAmmoContainer.transform.GetChild(i).GetComponent<TMP_Text>().text = Ammo[i].ToString();
        }
    }
    //Current weaopn image
    public void SetWeaponImage(int idx)
    {
        //loop through weapon images
        for (int i = 0; i < weaponSpriteContainer.transform.childCount; i++)
        {
            if (i == idx)
                //turn on current weaopn
                weaponSpriteContainer.transform.GetChild(i).gameObject.SetActive(true);
            else
                //turn off others
                weaponSpriteContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
        currentFirePoint = weaponSpriteContainer.transform.GetChild(idx).GetChild(0).GetComponent<Image>();
    }
    
    public void DisplayWeaponFire()
    {
        StartCoroutine("FireAnim");
    }

    private IEnumerator FireAnim()
    {
        RectTransform fireRect = currentFirePoint.GetComponent<RectTransform>();
        fireRect.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        float scaleVal = Random.Range(0.8f, 1.2f);
        fireRect.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
        currentFirePoint.enabled = true;
        yield return new WaitForSeconds(0.05f);
        currentFirePoint.enabled = false;
    }

    public void resetDistance() {
        playerMoveDistance = 0;
    }

    public void AnimateWeapon(float playerSpeed)
    {
        if (currentRecoilCoroutine == null) {
            playerMoveDistance += playerSpeed;
            weaponSpriteContainer.GetComponent<RectTransform>().position = weaponStartPos + new Vector3(_weaponAnimAmp * Mathf.Sin(_weaponAnimSpeed / 2 * playerMoveDistance), _weaponAnimAmp * Mathf.Sin(_weaponAnimSpeed * playerMoveDistance), 0f);
        }
    }

    public void AnimateRecoil()
    {
        Vector3 weaponEndPos = new Vector3(weaponStartPos.x + xRecoilTransformAmt, weaponStartPos.y, weaponStartPos.z);
        Vector3 weaponEndRotation = new Vector3(0, 0, -zRecoilRotationAmt);
        
        if (currentRecoilCoroutine != null) {
            StopCoroutine(currentRecoilCoroutine);
            currentRecoilCoroutine = null;
        }
        currentRecoilCoroutine = StartCoroutine(RecoilAnimation(weaponEndPos, weaponEndRotation, recoilAnimFrames));
    }

    public IEnumerator RecoilAnimation(Vector3 weaponEndPos, Vector3 weaponEndRotation, int frame)
    {
        while (frame > 0) {
            Vector3 newPosition = Vector3.Lerp(weaponStartPos, weaponEndPos, (float)frame/recoilAnimFrames);
            Vector3 newRotation = Vector3.Lerp(new Vector3(0,0,0), weaponEndRotation, (float)frame/recoilAnimFrames);
            weaponSpriteContainer.GetComponent<RectTransform>().position = newPosition;
            weaponSpriteContainer.GetComponent<RectTransform>().rotation = Quaternion.Euler(newRotation);
            frame--;
            yield return new WaitForSeconds(recoilAnimSpeed/recoilAnimFrames);
        }
        currentRecoilCoroutine = null;
    }

    //set icon background
    public void SetIconBG(int idx)
    {
        //loop through background objects for gun icons
        for (int i = 0; i < IconBGContainer.transform.childCount; i++)
        {
            if (i == idx)
                //set current weapon icon to be brighter
                IconBGContainer.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255);
            else
                //others to be darker
                IconBGContainer.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255, .2f);
        }
    }

    public void SetWeaponIcon(int ammoType, int stage)
    {
        //loop through weapon icon game objects for a stage
        for (int i = 0; i < weaponIconContainer.transform.GetChild(ammoType).childCount; i++)
        {
            //make held weapon visible
            if (i == stage) weaponIconContainer.transform.GetChild(ammoType).GetChild(i).gameObject.SetActive(true);
            //nonheld weapons invisible
            else weaponIconContainer.transform.GetChild(ammoType).GetChild(i).gameObject.SetActive(false);
        }
    }

    // Setting amount in mag
    public void LoadMagazineDisplay(Weapon weapon)
    {
        //stop current relaod cooldown bar
        if (currentCooldown != null) {
            StopCoroutine(currentCooldown);
            currentCooldown = null;
        }
        if (weapon.GetCooldownStatus())
        {
            //bring up new cooldown bar if new gun is reloading
            cooldownBar.enabled = true;
            currentCooldown = StartCoroutine(DisplayCooldown(weapon));
        }
        else cooldownBar.enabled = false;

        int ammo = weapon.ammo;
        int maxAmmo = weapon.magSize;

        //set up the ammo UI
        for (int i = 0; i < maxAmmo; i++)
        {
            magazineAmmoContainer.transform.GetChild(i).gameObject.SetActive(true);
            if (i < ammo)
            {
                //visible ammo icon
                magazineAmmoContainer.transform.GetChild(i).GetComponent<Image>().color = bulletColor;
            }
            else
            {
                //invisible ammo icon
                magazineAmmoContainer.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255, 0);
            }
        }
        for (int i = maxAmmo; i < magazineAmmoContainer.transform.childCount; i++)
        {
            //turn off ammo icons not in mag
            magazineAmmoContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    //reload cooldown bar
    public IEnumerator DisplayCooldown(Weapon weapon)
    {
        while (weapon.GetCooldownStatus())
        {
            cooldownBar.fillAmount = (Time.time - weapon.GetCooldownStartTime()) / weapon.cooldown;
            yield return null;
        }
        cooldownBar.enabled = false;
        currentCooldown = null;
    }

    // Setting upgrade
    public void SetUpgrades()
    {
        float[] slots = new float[3];
        InventoryManager.instance.playerInventory.GetUpgradeSlots(ref slots);

        //three bit identifiers
        for (int i =0; i<3; i++) {
            //check slot 3
            if (slots[i] >= 4) {
                slots[i]-= 4;
                upgradeSlots[i*3+2].enabled = true;
            }else
                upgradeSlots[i * 3 + 2].enabled = false;
            //check slot 2
            if (slots[i] >= 2)
            {
                slots[i] -= 2;
                upgradeSlots[i * 3 + 1].enabled = true;
            }
            else
                upgradeSlots[i * 3 + 1].enabled = false;
            //check slot 1
            if (slots[i] == 1)
            {
                slots[i]--;
                upgradeSlots[i * 3 + 0].enabled = true;
            }
            else
                upgradeSlots[i * 3 + 0].enabled = false;
        }
    }
    
    public void SetUpgrade(int upgradeType, float upgradeDuration)
    {
        if (currentTempCoroutine != null) {
            StopCoroutine(currentTempCoroutine);
            // Deactivate any running upgrades before starting another upgrade
            DeactivateUpgrades();
        }

        tempPickupCooldownBar.enabled = true;
        tempPickupCooldownBG.enabled = true;
        tempPickupText.enabled = true;

        Color tint;
        tint.a = 20;

        tempPickupCooldownBar.color = upgradeColors[upgradeType];
        tempPickupText.color = upgradeColors[upgradeType];

        // Activate upgrades and HUD tints
        switch (upgradeType)
        {
            // Armor
            case 0:
                healthBar.color = upgradeColors[upgradeType];
                tempPickupText.text = "Shields Active";
                PlayerController.instance.ActivateUpgrade(upgradeType);
                break;
            // Stimulant
            case 1:
                hudTint.enabled = true;
                tint = upgradeColors[upgradeType];
                tint.a = 0.01f;
                hudTint.color = tint;
                tempPickupText.text = "Speed-Up Active";
                PlayerController.instance.ActivateUpgrade(upgradeType);
                break;
            // Invisibility
            case 2:
                hudTint.enabled = true;
                tint = upgradeColors[upgradeType];
                tint.a = 0.01f;
                hudTint.color = tint;
                tempPickupText.text = "Invisibility Active";
                EnemyControllerParent.ActivateUpgrade();
                break;
            // Unlimited Magazine
            case 3:
                bulletColor = upgradeColors[upgradeType];
                LoadMagazineDisplay(WeaponActionController.instance.currentWeapon);
                tempPickupText.text = "Unlimited Ammo Active";
                WeaponActionController.instance.ActivateUpgrade();
                break;
        }

        currentTempCoroutine = StartCoroutine(TempUpgradeCountdown(upgradeDuration));
    }

    private IEnumerator TempUpgradeCountdown(float time)
    {
        float totalTime = time;

        while (time > 0)
        {
            time = time - Time.deltaTime;
            tempPickupCooldownBar.fillAmount = time/totalTime;
            yield return null;
        }
        tempPickupCooldownBar.enabled = false;
        tempPickupCooldownBG.enabled = false;
        tempPickupText.enabled = false;
        DeactivateUpgrades();
        currentTempCoroutine = null;
    }

    // This deactivates any running upgrades, and disables the tint image
    public void DeactivateUpgrades() {
        hudTint.enabled = false;
        // Deactivate armor upgrade
        PlayerController.instance.DeactivateUpgrade(0);
        healthBar.color = healthBarColor;
        // Deactivate stimulant upgrade
        PlayerController.instance.DeactivateUpgrade(1);
        // Deactivate invisibility upgrade
        EnemyControllerParent.DeactivateUpgrade();
        // Deactivate ammo upgrade
        WeaponActionController.instance.DeactivateUpgrade();
        bulletColor = Color.white;
        LoadMagazineDisplay(WeaponActionController.instance.currentWeapon);
    }

    public void IndicateDamage()
    {
        damageIndicator.enabled = true;
        if (!indicatingDamage) {
            indicatingDamage = true;
            StartCoroutine(DamageIndicatorAnim(damageIndicatorMaxAlpha, damageAnimLen / damageIndicatorMaxAlpha));
        }
    }

    private IEnumerator DamageIndicatorAnim(float transparency, float timeDelta)
    {
        while (transparency > 0)
        {
            damageIndicator.color = new Color(damageIndicator.color.r, damageIndicator.color.g, damageIndicator.color.b, transparency--/100);
            yield return new WaitForSecondsRealtime(timeDelta);
        }
        indicatingDamage = false;
        if (damageIndicator != null) damageIndicator.enabled = false;
    }

    public void DisplayPickupNotice(bool state) {
        if (state) {
            pickupNotice.SetActive(true);
        }
        else {
            pickupNotice.SetActive(false);
        }
    }
}
