import styled from 'styled-components'
import {
  color,
  flexbox,
  space,
  typography,
  ColorProps,
  FlexboxProps,
  SpaceProps,
  TypographyProps,
} from 'styled-system'

// eslint-disable-next-line
const Text = styled.span<
  ColorProps & FlexboxProps & SpaceProps & TypographyProps
>`
  ${color}
  ${flexbox}
  ${space}
  ${typography}
`

export default Text
