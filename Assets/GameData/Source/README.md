# Game data CSV workflow

Designers edit the shared Google Sheet or Excel workbook. Keep the column names and `id` values unchanged; export the relevant sheet as CSV and replace `Cards.csv` or `Enemies.csv` in this folder. IDs are permanent keys: do not derive them from display names or change them when names change.

In Unity, use **Tools > Game Data > Import Cards CSV** and **Tools > Game Data > Import Enemies CSV**. Review the Console for validation errors, then review the generated `.asset` diffs in version control. The importer updates matching assets by ID and creates new assets for new IDs. It never deletes assets when a row is removed from a CSV; remove data intentionally in Unity after checking references.

## Cards.csv

| Column | Rule |
| --- | --- |
| `id` | Required, unique, permanent identifier (example: `card_attack`). |
| `name` | Required display name. |
| `description` | Card description; commas are supported when the cell is quoted. |
| `type` | `Attack`, `Heal`, `Buff`, or `Shield`. |
| `cost`, `power`, `buffUses`, `buyPrice` | Required non-negative integers. |
| `illustrationKey` | Optional exact Unity Sprite name. It must match one Sprite uniquely; blank leaves the current reference unchanged. |

## Enemies.csv

| Column | Rule |
| --- | --- |
| `id` | Required, unique, permanent identifier (example: `enemy_boss`). |
| `name` | Required asset/display name. |
| `maxHealth` | Number greater than zero; decimal separator is `.`. |
| `attackPower`, `empowerAmount`, `blockAmount` | Required non-negative integers. |

The importer validates all rows before updating assets. CSV files should use one record per line; quoted cells and commas inside quoted cells are supported. Changes to the CSV are not reflected in the game until a developer imports them in the Unity Editor.
